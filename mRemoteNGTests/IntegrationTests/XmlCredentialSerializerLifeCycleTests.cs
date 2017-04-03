﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.IntegrationTests
{
    public class XmlCredentialSerializerLifeCycleTests
    {
        private ISerializer<IEnumerable<ICredentialRecord>, string> _serializer;
        private XmlCredentialRecordDeserializer _deserializer;
        private readonly Guid _id = Guid.NewGuid();
        private const string Title = "mycredential1";
        private const string Username = "user1";
        private const string Domain = "domain1";
        private readonly SecureString _password = "myPassword1!".ConvertToSecureString();

        [SetUp]
        public void Setup()
        {
            _serializer = new XmlCredentialRecordSerializer();
            _deserializer = new XmlCredentialRecordDeserializer();
        }

        [Test]
        public void WeCanSerializeAndDeserializeXmlCredentials()
        {
            var credentials = new[] { new CredentialRecord(), new CredentialRecord() };
            var serializedCredentials = _serializer.Serialize(credentials);
            var deserializedCredentials = _deserializer.Deserialize(serializedCredentials);
            Assert.That(deserializedCredentials.Count(), Is.EqualTo(2));
        }

        [Test]
        public void IdConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Id, Is.EqualTo(_id));
        }

        [Test]
        public void TitleConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Title, Is.EqualTo(Title));
        }

        [Test]
        public void UsernameConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Username, Is.EqualTo(Username));
        }

        [Test]
        public void DomainConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Domain, Is.EqualTo(Domain));
        }

        [Test]
        public void PasswordConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Password.ConvertToUnsecureString(), Is.EqualTo(_password.ConvertToUnsecureString()));
        }

        private ICredentialRecord SerializeThenDeserializeCredential()
        {
            var credentials = new[]
            {
                new CredentialRecord(_id) {Title = Title, Username = Username, Domain = Domain, Password = _password}
            };
            var serializedCredentials = _serializer.Serialize(credentials);
            var deserializedCredentials = _deserializer.Deserialize(serializedCredentials);
            return deserializedCredentials.First();
        }
    }
}