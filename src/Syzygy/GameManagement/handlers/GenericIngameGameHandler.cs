using amulware.Graphics;
using Lidgren.Network;
using Syzygy.Rendering;

namespace Syzygy.GameManagement
{
    abstract class GenericIngameGameHandler<TPeer> : GenericGameHandler<TPeer>, IGameDrawer
        where TPeer : NetPeer
    {
        protected readonly IStateContainer<TPeer> state;

        protected GenericIngameGameHandler(IStateContainer<TPeer> state)
            : base(state.Peer)
        {
            this.state = state;
        }

        public override void Update(UpdateEventArgs e)
        {
            base.Update(e);

            this.state.Game.Update(e);
        }

        public void Render(RenderManager renderMan)
        {
            renderMan.Render(this.state.View);
        }
    }
}
