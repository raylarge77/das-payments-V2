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
namespace SFA.DAS.Payments.PaymentsDue.AcceptanceTests.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Small Employer")]
    public partial class SmallEmployerFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SmallEmployers.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Small Employer", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 4
#line 5
 testRunner.Given("the current collection period is R13", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.And("the payments are for the current collection year", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.And("a learner is undertaking a training with a training provider", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.And("the SFA contribution percentage is 100%", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.And("planned course duration is 12 months", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "AimSeqNumber",
                        "ProgrammeType",
                        "FrameworkCode",
                        "PathwayCode",
                        "StandardCode",
                        "FundingLineType",
                        "LearnAimRef",
                        "TotalNegotiatedPrice",
                        "CompletionStatus"});
            table1.AddRow(new string[] {
                        "1",
                        "2",
                        "403",
                        "1",
                        "",
                        "16-18 Apprenticeship (From May 2017) Non-Levy Contract (non-procured)",
                        "ZPROG001",
                        "7500",
                        "completed"});
#line 10
 testRunner.And("the following course information:", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "PriceEpisodeIdentifier",
                        "Delivery Period",
                        "TransactionType",
                        "Amount"});
            table2.AddRow(new string[] {
                        "p1",
                        "1",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "2",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "3",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "4",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "5",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "6",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "7",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "8",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "9",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "10",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "11",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "12",
                        "Learning (TT1)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "12",
                        "Completion (TT2)",
                        "1500"});
#line 14
 testRunner.And("the following contract type 2 On Programme earnings are provided:", ((string)(null)), table2, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("AC1-Payment for a 16-18 non-DAS learner, small employer at start")]
        [NUnit.Framework.CategoryAttribute("SmallEmployerNonDas")]
        public virtual void AC1_PaymentForA16_18Non_DASLearnerSmallEmployerAtStart()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AC1-Payment for a 16-18 non-DAS learner, small employer at start", null, new string[] {
                        "SmallEmployerNonDas"});
#line 32
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 4
this.FeatureBackground();
#line 33
 testRunner.When("an earnings event is received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "PriceEpisodeIdentifier",
                        "Delivery Period",
                        "TransactionType",
                        "Amount"});
            table3.AddRow(new string[] {
                        "p1",
                        "1",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "2",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "3",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "4",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "5",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "6",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "7",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "8",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "9",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "10",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "11",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "12",
                        "Learning (TT1)",
                        "500"});
            table3.AddRow(new string[] {
                        "p1",
                        "12",
                        "Completion (TT2)",
                        "1500"});
#line 34
 testRunner.Then("the payments due component will generate the following contract type 2 payments d" +
                    "ue:", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("AC5- Payment for a 16-18 non-DAS learner, employer is not small")]
        public virtual void AC5_PaymentForA16_18Non_DASLearnerEmployerIsNotSmall()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AC5- Payment for a 16-18 non-DAS learner, employer is not small", null, ((string[])(null)));
#line 50
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 4
this.FeatureBackground();
#line 51
 testRunner.Given("the SFA contribution percentage is 90%", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 52
 testRunner.When("an earnings event is received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "PriceEpisodeIdentifier",
                        "Delivery Period",
                        "TransactionType",
                        "Amount"});
            table4.AddRow(new string[] {
                        "p1",
                        "1",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "2",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "3",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "4",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "5",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "6",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "7",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "8",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "9",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "10",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "11",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "12",
                        "Learning (TT1)",
                        "500"});
            table4.AddRow(new string[] {
                        "p1",
                        "12",
                        "Completion (TT2)",
                        "1500"});
#line 53
 testRunner.Then("the payments due component will generate the following contract type 2 payments d" +
                    "ue:", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
