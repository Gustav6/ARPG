using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class PathRequestManager
    {
        Queue<PathRequest> pathRequests = new();
        PathRequest currentRequest;

        private static PathRequestManager instance = new();

        private bool isProcessingPath;

        public static void RequestPath(Node pathStart, Node pathEnd, Action<Vector2[], bool> callback)
        {
            PathRequest newRequest = new(pathStart, pathEnd, callback);

            instance.pathRequests.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        private void TryProcessNext()
        {
            if (!isProcessingPath && pathRequests.Count > 0)
            {
                currentRequest = pathRequests.Dequeue();
                isProcessingPath = true;
            }
        }

        public void FinishedProcessingPath(Vector2[] path, bool success)
        {
            currentRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

        public struct PathRequest
        {
            public Node pathStart;
            public Node pathEnd;
            public Action<Vector2[], bool> callback;

            public PathRequest(Node start, Node end, Action<Vector2[], bool> callback)
            {
                pathStart = start;
                pathEnd = end;
                this.callback = callback;
            }
        }
    }
}
