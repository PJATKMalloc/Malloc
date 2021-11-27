/**
 * XRoute v1.0.0 - TSP Resolver for PostData
 * 
 * Copyright 2021 ANOVEI. All Rights Reserved
 * Author: Karol Szmajda (biuro@anovei.pl)
 */

using Itinero;

namespace Malloc.Data
{
    class XRouteResolver
    {
        #region Public Members
        public RouterPoint[] Points { get; private set; }

        /// <summary>
        /// Połączenia między punktami
        /// </summary>
        public int[] Connections { get; private set; }

        /// <summary>
        /// Dystans trasy w metrach
        /// </summary>
        public float Distance { get; private set; }

        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        #endregion

        #region Private Members
        private readonly int N;
        private readonly float[][] CostMatrix;
        #endregion

        #region Constructors
        public XRouteResolver(RouterPoint[] Points, int StartIndex, int EndIndex, float[][] CostMatrix)
        {
            this.N = Points.Length;
            this.Points = Points;
            this.StartIndex = StartIndex;
            this.EndIndex = EndIndex;
            this.CostMatrix = CostMatrix;

            this.Connections = NearestNeighbor();
        }
        #endregion

        /// <summary>
        /// Zwraca optymalną trasę
        /// </summary>
        /// <returns></returns>
        public void Solve()
        {
            Opt();
            this.Distance = CalculateDistance();
        }

        #region NearestNeighbor
        private int FindMinInRow(int Row, int[] trace)
        {
            float Min = float.MaxValue;
            int Index = -1;
            for (int x = 1; x < N - 1; x++)
            {
                if (!trace.Contains(x))
                {
                    if (CostMatrix[x][Row] != StartIndex && CostMatrix[x][Row] < Min)
                    {
                        Min = CostMatrix[x][Row];
                        Index = x;
                    }
                }
            }

            return Index;
        }

        private int FindMinInCol(int Col, int[] trace)
        {
            float Min = float.MaxValue;
            int Index = -1;
            for (int y = 1; y < N - 1; y++)
            {
                if (!trace.Contains(y))
                {
                    if (CostMatrix[Col][y] != StartIndex && CostMatrix[Col][y] < Min)
                    {
                        Min = CostMatrix[Col][y];
                        Index = y;
                    }
                }
            }

            return Index;
        }

        private int[] NearestNeighbor()
        {
            int[] Route = new int[N];
            int x = StartIndex, y = StartIndex;
            Route[0] = StartIndex;
            for (int i = 1; i < N - 1; i++)
            {
                if (i % 2 == 0)
                {
                    Route[i] = FindMinInCol(y, Route);
                    x = Route[i];
                }
                else
                {
                    Route[i] = FindMinInRow(x, Route);
                    y = Route[i];
                }

            }
            Route[N - 1] = EndIndex;

            return Route;
        }
        #endregion

        #region 2-Opt
        private int[] OptSwap(int[] Route, int i, int k)
        {
            int[] NewRoute = new int[Route.Length];
            for (int j = 0; j <= i - 1; j++)
            {
                NewRoute[j] = Route[j];
            }

            for (int j = i; j <= k; j++)
            {
                NewRoute[j] = Route[k - j + i];
            }

            for (int j = k + 1; j < Route.Length; j++)
            {
                NewRoute[j] = Route[j];
            }

            return NewRoute;
        }

        private void Opt()
        {
            float BestDistance = CalculateDistance(Connections);

        StartAgain:
            for (int i = 1; i < Connections.Length - 1; i++)
            {
                for (int k = i + 1; k < Connections.Length - 1; k++)
                {
                    int[] NewRoute = OptSwap(Connections, i, k);
                    float NewDistance = CalculateDistance(NewRoute);
                    if (NewDistance < BestDistance)
                    {
                        Connections = NewRoute;
                        BestDistance = NewDistance;
                        goto StartAgain;
                    }
                }
            }
        }
        #endregion

        #region HelperFuncs
  
        private float CalculateDistance(int[] Connections)
        {
            float Distance = 0;

            for (int i = 0; i < N - 1; ++i)
            {
                Distance += CostMatrix[Connections[i]][Connections[i + 1]];
            }

            return Distance;
        }

        private float CalculateDistance()
        {
            return CalculateDistance(Connections);
        }
        #endregion
    }
}
