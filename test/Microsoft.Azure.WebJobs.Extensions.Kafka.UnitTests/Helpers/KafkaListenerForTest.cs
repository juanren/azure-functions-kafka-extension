﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Confluent.Kafka;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.Kafka.UnitTests
{
    /// <summary>
    /// Test <see cref="KafkaListener{TKey, TValue}"/>, allowing the creation of a custom <see cref="IConsumer{TKey, TValue}"/>
    /// </summary>
    internal class KafkaListenerForTest<TKey, TValue> : KafkaListener<TKey, TValue>
    {
        private IConsumer<TKey, TValue> consumer;
        private ConsumerBuilder<TKey, TValue> consumerBuilder;

        public ConsumerConfig ConsumerConfig { get; private set; }

        public KafkaListenerForTest(ITriggeredFunctionExecutor executor,
            bool singleDispatch,
            KafkaOptions options,
            KafkaListenerConfiguration kafkaListenerConfiguration,
            bool requiresKey,
            IDeserializer<TValue> valueDeserializer,
            ILogger logger)
            : base(executor,
                singleDispatch,
                options,
                kafkaListenerConfiguration,
                requiresKey,
                valueDeserializer,
                logger)
        {
        }

        public void SetConsumerBuilder(ConsumerBuilder<TKey, TValue> consumerBuilder) => this.consumerBuilder = consumerBuilder;

        public void SetConsumer(IConsumer<TKey, TValue> consumer) => this.consumer = consumer;


        protected override ConsumerBuilder<TKey, TValue> CreateConsumerBuilder(ConsumerConfig config)
        {
            this.ConsumerConfig = config;
            return this.consumerBuilder ?? new TestConsumerBuilder<TKey, TValue>(config, this.consumer);
        }
    }
}
