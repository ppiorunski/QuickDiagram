using System;

namespace Codartis.SoftVis.Diagramming.Layout.ActionTracking.Implementation
{
    /// <summary>
    /// An action of a layout logic run that effects a LayoutVertex.
    /// </summary>
    internal class VertexAction : LayoutAction, IVertexAction
    {
        public LayoutVertexBase Vertex { get; }

        public VertexAction(string action, LayoutVertexBase vertex, double? amount = null)
            :base(action, amount)
        {
            if (vertex == null) throw new ArgumentNullException(nameof(vertex));

            Vertex = vertex;
        }

        public override string SubjectName => Vertex.ToString();
        public DiagramNode DiagramNode => (Vertex as DiagramNodeLayoutVertex)?.DiagramNode;

        private bool Equals(VertexAction other)
        {
            return base.Equals(other) && Equals(Vertex, other.Vertex);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VertexAction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (Vertex != null ? Vertex.GetHashCode() : 0);
            }
        }

        public static bool operator ==(VertexAction left, VertexAction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VertexAction left, VertexAction right)
        {
            return !Equals(left, right);
        }
    }
}