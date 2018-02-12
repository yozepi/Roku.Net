using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roku.Net.Tests.Unit
{
    public abstract class nSpecTestHarness : nspec
    {
        #region properties

        protected internal string Tags { get; set; }
        protected internal IFormatter Formatter { get; set; }
        protected bool FailFast { get; set; }
        internal Func<Type[]> specFinder { get; set; }

        #endregion //properties


        #region constructor

        public nSpecTestHarness()
        {
            this.Formatter = new ConsoleFormatter();
        }

        #endregion //constructor

        #region protected methods

        protected internal void LoadSpecs(Func<Type[]> specFinder)
        {
            this.specFinder = specFinder;
        }

        protected internal void RunSpecs()
        {
            var types = FindSpecTypes();
            var finder = new SpecFinder(types, "");
            var tagsFilter = new Tags().Parse(Tags);
            var builder = new ContextBuilder(finder, tagsFilter, new DefaultConventions());
            var runner = new ContextRunner(tagsFilter, Formatter, FailFast);
            var contexts = builder.Contexts().Build();

            bool hasFocusTags = contexts.AnyTaggedWithFocus();
            if (hasFocusTags)
            {
                tagsFilter = new Tags().Parse(NSpec.Domain.Tags.Focus);

                builder = new ContextBuilder(finder, tagsFilter, new DefaultConventions());

                runner = new ContextRunner(tagsFilter, Formatter, FailFast);

                contexts = builder.Contexts().Build();
            }


            var results = runner.Run(contexts);

            //assert that there aren't any failures
            Assert.AreEqual<int>(0, results.Failures().Count());

            var pending = results.Examples().Count(xmpl => xmpl.Pending);
            if (pending != 0)
            {
                Assert.Inconclusive("{0} spec(s) are marked as pending.", pending);
            }
            if (results.Examples().Count() == 0)
            {
                Assert.Inconclusive("Spec count is zero.");
            }
            if (hasFocusTags)
            {
                Assert.Inconclusive("One or more specs are tagged with focus.");
            }
        }

        protected internal void RunSpecs(Func<Type[]> specFinder)
        {
            LoadSpecs(specFinder);
            RunSpecs();
        }

        protected internal void RunMySpecs()
        {
            LoadSpecs(() => new Type[] { this.GetType() });
            RunSpecs();
        }

        #endregion //protected methods

        #region helpers

        private Type[] FindSpecTypes()
        {
            if (specFinder != null)
            {
                return specFinder();
            }
            return GetType().Assembly.GetTypes();
        }

        #endregion //helpers
    }
}
