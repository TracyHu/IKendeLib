<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="d283e446-3fc8-4638-a81f-7d0b5a0247a3" namespace="Beetle.Tracker" xmlSchemaNamespace="urn:Beetle.Tracker" assemblyName="Beetle.Tracker" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="TrackerServerSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="trackerServerSection">
      <elementProperties>
        <elementProperty name="Trackers" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="trackers" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/d283e446-3fc8-4638-a81f-7d0b5a0247a3/AppTrackerConfigCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="AppTrackerConfig">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d283e446-3fc8-4638-a81f-7d0b5a0247a3/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d283e446-3fc8-4638-a81f-7d0b5a0247a3/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="AppTrackerConfigCollection" collectionType="AddRemoveClearMapAlternate" xmlItemName="appTrackerConfig" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/d283e446-3fc8-4638-a81f-7d0b5a0247a3/AppTrackerConfig" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>