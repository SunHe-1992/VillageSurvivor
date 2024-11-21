using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunHeTBS
{
    public interface IEntityUpdate
    {
        void Update();

    }
    public abstract class GameEntity
    {
        public virtual void Update(float dt)
        {
        }
    }
}
