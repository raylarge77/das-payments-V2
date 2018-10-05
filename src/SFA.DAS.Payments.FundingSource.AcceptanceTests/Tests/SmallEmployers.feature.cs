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
namespace SFA.DAS.Payments.FundingSource.AcceptanceTests.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("non-DAS learner employed with a small employer, is fully funded for on programme " +
        "and completion payments")]
    public partial class Non_DASLearnerEmployedWithASmallEmployerIsFullyFundedForOnProgrammeAndCompletionPaymentsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SmallEmployers.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "non-DAS learner employed with a small employer, is fully funded for on programme " +
                    "and completion payments", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 3
#line 4
 testRunner.Given("the current collection period is R1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
 testRunner.And("the payments are for the current collection year", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 6
 testRunner.And("a learner is undertaking a training with a training provider", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.And("the SFA contribution percentage is 100%", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "PriceEpisodeIdentifier",
                        "Delivery Period",
                        "TransactionType",
                        "Amount"});
            table1.AddRow(new string[] {
                        "p1",
                        "1",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "2",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "3",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "4",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "5",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "6",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "7",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "8",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "9",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "10",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "11",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "12",
                        "Learning (TT1)",
                        "500"});
            table1.AddRow(new string[] {
                        "p1",
                        "12",
                        "Completion (TT2)",
                        "1500"});
#line 8
 testRunner.And("the required payments component generates the following contract type 2 payable e" +
                    "arnings:", ((string)(null)), table1, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("AC1-Payment for a 16-18 non-DAS learner, small employer at start")]
        [NUnit.Framework.CategoryAttribute("SmallEmployerNonDas")]
        public virtual void AC1_PaymentForA16_18Non_DASLearnerSmallEmployerAtStart()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AC1-Payment for a 16-18 non-DAS learner, small employer at start", null, new string[] {
                        "SmallEmployerNonDas"});
#line 26
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line 27
 testRunner.When("required payments event is received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "PriceEpisodeIdentifier",
                        "Delivery Period",
                        "TransactionType",
                        "FundingSource",
                        "Amount"});
            table2.AddRow(new string[] {
                        "p1",
                        "1",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "2",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "3",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "4",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "5",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "6",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "7",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "8",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "9",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "10",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "11",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "12",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "500"});
            table2.AddRow(new string[] {
                        "p1",
                        "12",
                        "Completion (TT2)",
                        "CoInvestedSfa (FS2)",
                        "1500"});
#line 28
 testRunner.Then("the payment source component will generate the following contract type 2 coinvest" +
                    "ed payments:", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("AC5- Payment for a 16-18 non-DAS learner, employer is not small")]
        public virtual void AC5_PaymentForA16_18Non_DASLearnerEmployerIsNotSmall()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AC5- Payment for a 16-18 non-DAS learner, employer is not small", null, ((string[])(null)));
#line 44
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line 45
 testRunner.Given("the SFA contribution percentage is 90%", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 46
 testRunner.When("required payments event is received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "PriceEpisodeIdentifier",
                        "Delivery Period",
                        "TransactionType",
                        "FundingSource",
                        "Amount"});
            table3.AddRow(new string[] {
                        "p1",
                        "1",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "2",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "3",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "4",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "5",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "6",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "7",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "8",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "9",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "10",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "11",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "12",
                        "Learning (TT1)",
                        "CoInvestedSfa (FS2)",
                        "450"});
            table3.AddRow(new string[] {
                        "p1",
                        "12",
                        "Completion (TT2)",
                        "CoInvestedSfa (FS2)",
                        "1350"});
            table3.AddRow(new string[] {
                        "p1",
                        "1",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "2",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "3",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "4",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "5",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "6",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "7",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "8",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "9",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "10",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "11",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "12",
                        "Learning (TT1)",
                        "CoInvestedEmployer (FS3)",
                        "50"});
            table3.AddRow(new string[] {
                        "p1",
                        "12",
                        "Completion (TT2)",
                        "CoInvestedEmployer (FS3)",
                        "150"});
#line 47
 testRunner.Then("the payment source component will generate the following contract type 2 coinvest" +
                    "ed payments:", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
