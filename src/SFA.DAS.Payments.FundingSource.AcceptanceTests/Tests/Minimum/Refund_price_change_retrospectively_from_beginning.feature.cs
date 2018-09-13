﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.DAS.Payments.FundingSource.AcceptanceTests.Tests.Minimum
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Refunds - Provider earnings and payments where learner refund payments are due")]
    public partial class Refunds_ProviderEarningsAndPaymentsWhereLearnerRefundPaymentsAreDueFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Refund_price_change_retrospectively_from_beginning.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Refunds - Provider earnings and payments where learner refund payments are due", "\t\t 894-AC02 - non DAS standard learner, payments made then price is changed retro" +
                    "spectively from beginning", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 9
#line 10
 testRunner.Given("the current processing period is 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 12
 testRunner.And("a learner with LearnRefNumber learnref1 and Uln 10000 undertaking training with t" +
                    "raining provider 10000", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table56 = new TechTalk.SpecFlow.Table(new string[] {
                        "PriceEpisodeIdentifier",
                        "Period",
                        "ULN",
                        "TransactionType",
                        "Amount"});
            table56.AddRow(new string[] {
                        "p1",
                        "1",
                        "10000",
                        "1",
                        "-750"});
            table56.AddRow(new string[] {
                        "p1",
                        "2",
                        "10000",
                        "1",
                        "-750"});
            table56.AddRow(new string[] {
                        "p2",
                        "1",
                        "10000",
                        "1",
                        "0.6667"});
            table56.AddRow(new string[] {
                        "p2",
                        "2",
                        "10000",
                        "1",
                        "0.6667"});
            table56.AddRow(new string[] {
                        "p2",
                        "3",
                        "10000",
                        "1",
                        "0.6667"});
#line 14
 testRunner.And("the payments due component generates the following contract type 2 payable earnin" +
                    "gs:", ((string)(null)), table56, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Contract Type 2 Learning payment")]
        [NUnit.Framework.CategoryAttribute("Non-DAS")]
        [NUnit.Framework.CategoryAttribute("minimum_tests")]
        [NUnit.Framework.CategoryAttribute("Refunds")]
        [NUnit.Framework.CategoryAttribute("price_reduced_retrospectively")]
        [NUnit.Framework.CategoryAttribute("partial")]
        public virtual void ContractType2LearningPayment()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Contract Type 2 Learning payment", null, new string[] {
                        "Non-DAS",
                        "minimum_tests",
                        "Refunds",
                        "price_reduced_retrospectively",
                        "partial"});
#line 28
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
this.FeatureBackground();
#line 30
 testRunner.When("MASH is received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table57 = new TechTalk.SpecFlow.Table(new string[] {
                        "LearnRefNumber",
                        "Ukprn",
                        "PriceEpisodeIdentifier",
                        "Period",
                        "ULN",
                        "TransactionType",
                        "FundingSource",
                        "Amount"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "1",
                        "10000",
                        "Learning_1",
                        "CoInvestedSfa_2",
                        "-675"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "1",
                        "10000",
                        "Learning_1",
                        "CoInvestedEmployer_3",
                        "-75"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "2",
                        "10000",
                        "Learning_1",
                        "CoInvestedSfa_2",
                        "-675"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "2",
                        "10000",
                        "Learning_1",
                        "CoInvestedEmployer_3",
                        "-75"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p2",
                        "1",
                        "10000",
                        "Learning_1",
                        "CoInvestedSfa_2",
                        "0.60"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p2",
                        "1",
                        "10000",
                        "Learning_1",
                        "CoInvestedEmployer_3",
                        "0.0667"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p2",
                        "2",
                        "10000",
                        "Learning_1",
                        "CoInvestedSfa_2",
                        "0.60"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p2",
                        "2",
                        "10000",
                        "Learning_1",
                        "CoInvestedEmployer_3",
                        "0.0667"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p2",
                        "3",
                        "10000",
                        "Learning_1",
                        "CoInvestedSfa_2",
                        "0.60"});
            table57.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p2",
                        "3",
                        "10000",
                        "Learning_1",
                        "CoInvestedEmployer_3",
                        "0.0667"});
#line 32
 testRunner.Then("the payment source component will generate the following contract type 2 coinvest" +
                    "ed payments:", ((string)(null)), table57, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
