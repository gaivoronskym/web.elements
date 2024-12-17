namespace Web.Elements;

public interface IExit
{
    bool Ready();

    public sealed class Never : IExit
    {
        public bool Ready()
        {
            return false;
        }
    }

    public sealed class Or : IExit
    {
        private readonly IExit left;
        private readonly IExit right;

        public Or(IExit left, IExit right)
        {
            this.left = left;
            this.right = right;
        }

        public bool Ready()
        {
            return left.Ready() || left.Ready();
        }
    }

    public sealed class And : IExit
    {
        private readonly IExit left;
        private readonly IExit right;

        public And(IExit left, IExit right)
        {
            this.left = left;
            this.right = right;
        }

        public bool Ready()
        {
            return left.Ready() && left.Ready();
        }
    }

    public sealed class Not : IExit
    {
        private readonly IExit origin;

        public Not(IExit origin)
        {
            this.origin = origin;
        }

        public bool Ready()
        {
            return !origin.Ready();
        }
    }
}