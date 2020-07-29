using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PostSharp.Samples.StoredProcedure
{
  class SpeakerApi : BaseDbApi
  {
    public SpeakerApi(SqlConnection connection, SqlTransaction transaction = null) : base(connection, transaction)
    {
    }


    // Sync methods.
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern IEnumerable<Speaker> GetActiveSpeakers();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern IEnumerable<Speaker> GetSpeakers();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetSpeakerStatus(int id, bool isActive);


    // Async variants of the same methods.
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern IAsyncEnumerable<Speaker> GetActiveSpeakersAsync();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern IAsyncEnumerable<Speaker> GetSpeakersAsync();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Task SetSpeakerStatusAsync(int id, bool isActive);
  }

}
