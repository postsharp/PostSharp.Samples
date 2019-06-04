using PostSharp.Patterns.Caching.Dependencies;
using System;

namespace PostSharp.Samples.Caching
{
  [Serializable]
  internal class Account : ICacheDependency
  {
    public int AccountId;

    public string GetCacheKey()
    {
      // Note that this key should be globally unique, not just unique within the Account class.
      return $"Account:{AccountId}";
    }

    public bool Equals(ICacheDependency other)
    {
      // TODO: Remove this method. Future builds of PostSharp will not need to implement it.
      throw new NotImplementedException();
    }
  }
}