using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuMVC.Core.Assets.Combination
{
    // TODO -- gotta hit this with integration tests.  Hard.
    public class CombinationDeterminationService : ICombinationDeterminationService
    {
        private readonly IAssetCombinationCache _cache;
        private readonly IEnumerable<ICombinationPolicy> _policies;

        public CombinationDeterminationService(IAssetCombinationCache cache, IEnumerable<ICombinationPolicy> policies)
        {
            _cache = cache;
            _policies = policies;
        }

        public void TryToReplaceWithCombinations(AssetTagPlan plan)
        {
            TryAllExistingCombinations(plan);

            tryCombinationCandidatesAndPolicies(plan);
        }

        private void tryCombinationCandidatesAndPolicies(AssetTagPlan plan)
        {
            var mimeTypePolicies = _policies.Where(x => x.MimeType == plan.MimeType);
            var combinationPolicies = _cache.OrderedCombinationCandidatesFor(plan.MimeType).Union(mimeTypePolicies);
            combinationPolicies.Each(policy => ExecutePolicy(plan, policy));
        }

        public virtual void ExecutePolicy(AssetTagPlan plan, ICombinationPolicy policy)
        {
            policy.DetermineCombinations(plan).Each(combo => _cache.StoreCombination(plan.MimeType, combo));
        }

        public void TryAllExistingCombinations(AssetTagPlan plan)
        {
            _cache.OrderedListOfCombinations(plan.MimeType).Each(combo => plan.TryCombination(combo));
        }
    }
}