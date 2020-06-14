using Godot;

namespace SpacedRocks.Common
{
    public class ScreenWrap: Node
    {
        private Vector2 _screenSize;
        private int _screenBuffer;

        public ScreenWrap(Vector2 screenSize, int screenBuffer)
        {
            _screenSize = screenSize;
            _screenBuffer = screenBuffer;
        }

        public Vector2 WrappedPosition(Vector2 position)
        {
            var wrappedX = Mathf.Wrap(position.x, -_screenBuffer, _screenSize.x + _screenBuffer);
            var wrappedY = Mathf.Wrap(position.y, -_screenBuffer, _screenSize.y + _screenBuffer);
            return new Vector2(wrappedX, wrappedY);
        }

        public Vector2 WrappedPosition(Vector2 position, Vector2 spriteSize)
        {
            var pos = position;
            if (pos.x > _screenSize.x + spriteSize.x)
                pos.x = -spriteSize.x + 10;
            if (pos.x < -spriteSize.x)
                pos.x = _screenSize.x + spriteSize.x - 10;
            if (pos.y > _screenSize.y + spriteSize.y - 10)
                pos.y = -spriteSize.y;
            if (pos.y < -spriteSize.y)
                pos.y = _screenSize.y + spriteSize.y - 10;
            return pos;
        }
    }
}