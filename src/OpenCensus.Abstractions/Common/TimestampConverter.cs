﻿// <copyright file="TimestampConverter.cs" company="OpenCensus Authors">
// Copyright 2018, OpenCensus Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

namespace OpenCensus.Internal
{
    using OpenCensus.Common;

    /// <summary>
    /// Converts nanoseconds into timestamp.
    /// </summary>
    public class TimestampConverter
    {
        private readonly Timestamp timestamp;
        private readonly long nanoTime;

        private TimestampConverter(Timestamp timestamp, long nanoTime)
        {
            this.timestamp = timestamp;
            this.nanoTime = nanoTime;
        }

        // Returns a WallTimeConverter initialized to now.
        public static TimestampConverter Now(IClock clock)
        {
            return new TimestampConverter(clock.Now, clock.NowNanos);
        }

        /// <summary>
        /// Converts nanoseconds to the timestamp.
        /// </summary>
        /// <param name="nanoTime">Nanoseconds time.</param>
        /// <returns>Timestamp from the nanoseconds.</returns>
        public Timestamp ConvertNanoTime(long nanoTime)
        {
            return this.timestamp.AddNanos(nanoTime - this.nanoTime);
        }
    }
}
