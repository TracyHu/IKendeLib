<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="0d8145c3-5f29-4fec-a88a-21904cda2482" namespace="IKende.com.SNService" xmlSchemaNamespace="urn:IKende.com.SNService" assemblyName="IKende.com.SNService" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="SNServiceSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="sNServiceSection">
      <elementProperties>
        <elementProperty name="Sequence" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="sequence" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/0d8145c3-5f29-4fec-a88a-21904cda2482/SequenceConf" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="SequenceConf">
      <attributeProperties>
        <attributeProperty name="Start" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="start" isReadOnly="false" defaultValue="1">
          <type>
            <externalTypeMoniker name="/0d8145c3-5f29-4fec-a88a-21904cda2482/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Step" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="step" isReadOnly="false" defaultValue="1">
          <type>
            <externalTypeMoniker name="/0d8145c3-5f29-4fec-a88a-21904cda2482/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>