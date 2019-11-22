﻿using Newtonsoft.Json;
using SFA.DAS.Payments.DataLocks.Domain.Models;
using SFA.DAS.Payments.DataLocks.Domain.Services.PriceEpidodeChanges;
using SFA.DAS.Payments.DataLocks.Messages.Events;
using SFA.DAS.Payments.DataLocks.Model.Entities;
using SFA.DAS.Payments.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Payments.DataLocks.Application.Repositories;
using SFA.DAS.Payments.Model.Core.Entities;

namespace SFA.DAS.Payments.DataLocks.Application.Services
{

    public interface IPriceEpisodesReceivedService
    {
        Task<List<PriceEpisodeStatusChange>> JobSucceeded(long jobId, long ukprn);
    }


    public class PriceEpisodesReceivedService: IPriceEpisodesReceivedService
    {
        private readonly ICurrentPriceEpisodeForJobStore currentPriceEpisodesStore;
        private readonly IReceivedDataLockEventStore receivedEventStore;
        private readonly IPriceEpisodeStatusChangeBuilder statusChangeBuilder;

        public PriceEpisodesReceivedService(ICurrentPriceEpisodeForJobStore store,
            IReceivedDataLockEventStore receivedEventStore,
            IPriceEpisodeStatusChangeBuilder statusChangeBuilder)
        {
            currentPriceEpisodesStore = store;
            this.receivedEventStore = receivedEventStore;
            this.statusChangeBuilder = statusChangeBuilder;
        }

        public async Task<List<PriceEpisodeStatusChange>> JobSucceeded(long jobId, long ukprn)
        {
            var dataLockEvents = (await GetDataLocks(jobId, ukprn)).ToList();

            var currentPriceEpisodes = await GetCurrentPriceEpisodes(jobId, ukprn);

            var changes = CalculatePriceEpisodeStatus(dataLockEvents, currentPriceEpisodes);

            var buildEvents = await CreateStatusChangedEvents(dataLockEvents, changes);

            await ReplaceCurrentPriceEpisodes(jobId, ukprn, dataLockEvents);

            await RemoveReceivedDataLockEvents(jobId, ukprn);

            return buildEvents;
        }

        private async Task<IEnumerable<DataLockEvent>> GetDataLocks(long jobId, long ukprn)
        {
            var receivedEvents = await receivedEventStore.GetDataLocks(jobId, ukprn);
            var datalocks = receivedEvents.Select(x =>
            {
                var type = Type.GetType(typeof(PayableEarningEvent).AssemblyQualifiedName);
                return (DataLockEvent)JsonConvert.DeserializeObject(x.Message, type);
            });
            return datalocks;
        }

        private Task<IEnumerable<CurrentPriceEpisode>> GetCurrentPriceEpisodes(long jobId, long ukprn)
        {
            return currentPriceEpisodesStore.GetCurrentPriceEpisodes(jobId, ukprn);
        }

        private static List<(string identifier, PriceEpisodeStatus status)> CalculatePriceEpisodeStatus(
            IEnumerable<DataLockEvent> dataLocks, IEnumerable<CurrentPriceEpisode> currentPriceEpisodes)
        {
            var calculator = new PriceEpisodeStatusCalculator();
            var changes = calculator.Calculate(currentPriceEpisodes, dataLocks.SelectMany(x => x.PriceEpisodes));
            return changes;
        }

        private async Task<List<PriceEpisodeStatusChange>> CreateStatusChangedEvents(
            IEnumerable<DataLockEvent> dataLocks, List<(string identifier, PriceEpisodeStatus status)> changes)
        {
            return  await statusChangeBuilder.Build(dataLocks.ToList(), changes);
        }

        private async Task ReplaceCurrentPriceEpisodes(
            long jobId, long ukprn, IEnumerable<DataLockEvent> dataLocks)
        {
            var replacement = dataLocks
                .SelectMany(x => x.PriceEpisodes, (dlock, episode) =>
                    new CurrentPriceEpisode
                    {
                        JobId = jobId,
                        Ukprn = ukprn,
                        Uln = dlock.Learner.Uln,
                        PriceEpisodeIdentifier = episode.Identifier,
                        AgreedPrice = episode.AgreedPrice,
                    });

            await currentPriceEpisodesStore.Replace(jobId, ukprn, replacement);
        }

        private Task RemoveReceivedDataLockEvents(long jobId, long ukprn)
        {
            return receivedEventStore.Remove(jobId, ukprn);
        }
    }
}
