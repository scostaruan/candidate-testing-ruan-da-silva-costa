using CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Interfaces;

namespace CandidateTesting.RuanDaSilvaCosta.AgoraConverter.Adapter
{
    public class AgoraAdapter : IMinhaCdn
    {
        Agora Agora;

        public AgoraAdapter(Agora agora)
        {
            Agora = agora;
        }

        public void CreateLog()
        {
            Agora.CreateLog();
        }
    }
}
