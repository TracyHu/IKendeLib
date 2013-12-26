<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="5f80d0c1-0130-41de-ae78-1b000ffa8948" namespace="Beetle.Server.Core" xmlSchemaNamespace="urn:Beetle.Server.Core" assemblyName="Beetle.Server.Core" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="BeetleServerSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="beetleServerSection">
      <attributeProperties>
        <attributeProperty name="ServerName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="serverName" isReadOnly="false" defaultValue="&quot;Beetle Service&quot;">
          <type>
            <externalTypeMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="DisplayName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="displayName" isReadOnly="false" defaultValue="&quot;Beetle Service&quot;">
          <type>
            <externalTypeMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Servers" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="servers" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/BeetleServerCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="BeetleServerConf">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Host" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="host" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Port" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="port" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Package" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="package" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Handler" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="handler" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="BeetleServerCollection" collectionType="AddRemoveClearMapAlternate" xmlItemName="beetleServerConf" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/5f80d0c1-0130-41de-ae78-1b000ffa8948/BeetleServerConf" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>