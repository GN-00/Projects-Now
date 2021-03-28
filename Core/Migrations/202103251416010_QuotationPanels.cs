namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuotationPanels : DbMigration
    {
		public override void Up()
		{

			CreateTable("Quotation._Panels",
				c => new {
					Id = c.Int(nullable: false, identity: true),
					RevisePanelId = c.Int(nullable: true),
					QuotationId = c.Int(nullable: false),
					SN = c.Int(nullable: true),
					Name = c.String(nullable: false, maxLength: 256),
					Qty = c.Int(nullable: false),
					Type = c.String(nullable: false, maxLength: 100),
					Profit = c.Double(nullable: false),
					EnclosureName = c.String(maxLength: 100),
					EnclosureType = c.String(maxLength: 50),
					EnclosureInstallation = c.String(maxLength: 50),
					EnclosureHeight = c.Double(),
					EnclosureWidth = c.Double(),
					EnclosureDepth = c.Double(),
					EnclosureIP = c.String(maxLength: 5),
					EnclosureLocation = c.String(maxLength: 50),
					EnclosureColor = c.String(maxLength: 50),
					EnclosureMetalType = c.String(maxLength: 50),
					EnclosureForm = c.String(maxLength: 10),
					EnclosureDoor = c.String(maxLength: 50),
					EnclosureFunctional = c.String(maxLength: 50),

					Source = c.String(maxLength: 50),
					Icu = c.Double(),
					Frequency = c.String(maxLength: 50),
					PowerSupplyOperation = c.String(maxLength: 50),
					EarthingSystem = c.String(maxLength: 50),
					DirectCurrent = c.String(maxLength: 50),

					Busbar = c.String(maxLength: 50),
					BusbarHorizontal = c.String(maxLength: 50),
					BusbarVertical = c.String(maxLength: 50),
					NeutralSize = c.String(maxLength: 50),
					EarthSize = c.String(maxLength: 50),

					SignallingVoltage = c.String(maxLength: 50),
					SignallingSource = c.String(maxLength: 50),
					ControlVoltage = c.String(maxLength: 50),
					ControlSource = c.String(maxLength: 50),
					SensorsVoltage = c.String(maxLength: 50),
					SensorsSource = c.String(maxLength: 50),
					LightingVoltage = c.String(maxLength: 50),
					LightingSource = c.String(maxLength: 50),

					ClimateType = c.String(maxLength: 50),
					AtmosphereType = c.String(maxLength: 50),
					PollutionRisks = c.String(maxLength: 50),
					AmbientTemperature = c.Double(),
					RelativeHumidity = c.Double(),
					SeaLevel = c.Double(),

					IndicationType = c.String(maxLength: 50),
					IndicationSize = c.String(maxLength: 50),
					IndicationOther = c.String(maxLength: 50),
					MeterRelay = c.String(maxLength: 50),

					IncomingFixed = c.Boolean(),
					IncomingPlugIn = c.Boolean(),
					IncomingDrawout = c.Boolean(),
					IncomingMotorized = c.Boolean(),
					IncomingBehindDoor = c.Boolean(),
					IncomingThroughPlate = c.Boolean(),
					IncomingInterLock = c.Boolean(),
					IncomingPadlocking = c.Boolean(),
					IncomingShutter = c.Boolean(),
					IncomingThroughDoor = c.Boolean(),
					IncomingThroughCover = c.Boolean(),
					IncomingDirect = c.Boolean(),
					IncomingTerminalBlocks = c.Boolean(),
					IncomingBusbarLinks = c.Boolean(),
					IncomingFront = c.Boolean(),
					IncomingRear = c.Boolean(),
					IncomingLeftRight = c.Boolean(),
					IncomingTopCables = c.Boolean(),
					IncomingTopBusduct = c.Boolean(),
					IncomingBottomCables = c.Boolean(),
					IncomingScrews = c.Boolean(),
					IncomingGlandPlate = c.Boolean(),
					IncomingCableGland = c.Boolean(),
					IncomingShrouding = c.Boolean(),

					OutgoingFixed = c.Boolean(),
					OutgoingPlugIn = c.Boolean(),
					OutgoingDrawout = c.Boolean(),
					OutgoingMotorized = c.Boolean(),
					OutgoingBehindDoor = c.Boolean(),
					OutgoingThroughPlate = c.Boolean(),
					OutgoingInterLock = c.Boolean(),
					OutgoingPadlocking = c.Boolean(),
					OutgoingShutter = c.Boolean(),
					OutgoingThroughDoor = c.Boolean(),
					OutgoingThroughCover = c.Boolean(),
					OutgoingDirect = c.Boolean(),
					OutgoingTerminalBlocks = c.Boolean(),
					OutgoingBusbarLinks = c.Boolean(),
					OutgoingFront = c.Boolean(),
					OutgoingRear = c.Boolean(),
					OutgoingLeftRight = c.Boolean(),
					OutgoingTopCables = c.Boolean(),
					OutgoingTopBusduct = c.Boolean(),
					OutgoingBottomCables = c.Boolean(),
					OutgoingScrews = c.Boolean(),
					OutgoingGlandPlate = c.Boolean(),
					OutgoingCableGland = c.Boolean(),
					OutgoingShrouding = c.Boolean(),

					PushButtonON = c.String(maxLength: 50),
					PushButtonOFF = c.String(maxLength: 50),
					PushButtonReset = c.String(maxLength: 50),
					SignallingON = c.String(maxLength: 50),
					SignallingOFF = c.String(maxLength: 50),
					SignallingTrip = c.String(maxLength: 50),

					ExternalLabelType = c.String(maxLength: 50),
					ExternalLabelFixeing = c.String(maxLength: 50),
					ExternalLabelLanguage = c.String(maxLength: 50),
					InternalLabelType = c.String(maxLength: 50),
					InternalLabelFixeing = c.String(maxLength: 50),
					InternalLabelLanguage = c.String(maxLength: 50),
					EquipmentLabelType = c.String(maxLength: 50),
					EquipmentLabelFixeing = c.String(maxLength: 50),
					EquipmentLabelLanguage = c.String(maxLength: 50),
					LabelBackground = c.String(maxLength: 50),
					LabelFont = c.String(maxLength: 50),

					AuxiliaryVoltageSection = c.Double(),
					AuxiliaryVoltageColor = c.String(maxLength: 50),
					AuxiliaryVoltageType = c.String(maxLength: 50),
					AuxiliaryCurrentSection = c.Double(),
					AuxiliaryCurrentColor = c.String(maxLength: 50),
					AuxiliaryCurrentType = c.String(maxLength: 50),

					ApparatusDefind = c.String(maxLength: 50),
					Weight = c.String(maxLength: 50),
					ForInformation = c.Boolean(),
					ForProduction = c.Boolean(),
					ForApproval = c.Boolean(),
					AsManufactured = c.Boolean(),
					Remarks = c.String(maxLength: 250),
				})
				.PrimaryKey(t => t.Id)
				.Index(t => t.Id)
				.Index(t => t.QuotationId)
				.Index(t => t.RevisePanelId);

			Sql($"SET IDENTITY_INSERT Quotation._Panels ON; " +
				$"Insert Into Quotation._Panels " +
			    $"([Id], " +
			    $"[RevisePanelId], " +
			    $"[QuotationId], " +
			    $"[SN], " +
			    $"[Name], " +
			    $"[Qty], " +
			    $"[Type], " +
			    $"[Profit], " +
			    $"[EnclosureName], " +
			    $"[EnclosureType], " +
			    $"[EnclosureLocation], " +
			    $"[EnclosureInstallation], " +
			    $"[EnclosureHeight], " +
			    $"[EnclosureWidth], " +
			    $"[EnclosureDepth], " +
			    $"[EnclosureIP], " +
			    $"[EnclosureColor], " +
			    $"[EnclosureMetalType], " +
			    $"[EnclosureForm], " +
			    $"[EnclosureDoor], " +
			    $"[EnclosureFunctional], " +
			    $"[Source], " +
			    $"[Icu], " +
			    $"[Frequency], " +
			    $"[PowerSupplyOperation], " +
			    $"[EarthingSystem], " +
			    $"[DirectCurrent], " +
			    $"[Busbar], " +
			    $"[BusbarHorizontal], " +
			    $"[BusbarVertical], " +
			    $"[NeutralSize], " +
			    $"[EarthSize], " +
			    $"[SignallingVoltage], " +
			    $"[SignallingSource], " +
			    $"[ControlVoltage], " +
			    $"[ControlSource], " +
			    $"[SensorsVoltage], " +
			    $"[SensorsSource], " +
			    $"[LightingVoltage], " +
			    $"[LightingSource], " +
			    $"[ClimateType], " +
			    $"[AtmosphereType], " +
			    $"[PollutionRisks], " +
			    $"[AmbientTemperature], " +
			    $"[RelativeHumidity], " +
			    $"[SeaLevel], " +
			    $"[IndicationType], " +
			    $"[IndicationSize], " +
			    $"[IndicationOther], " +
			    $"[MeterRelay], " +
			    $"[IncomingFixed], " +
			    $"[IncomingPlugIn], " +
			    $"[IncomingDrawout], " +
			    $"[IncomingMotorized], " +
			    $"[IncomingBehindDoor], " +
			    $"[IncomingThroughPlate], " +
			    $"[IncomingInterLock], " +
			    $"[IncomingPadlocking], " +
			    $"[IncomingShutter], " +
			    $"[IncomingThroughDoor], " +
			    $"[IncomingThroughCover], " +
			    $"[IncomingDirect], " +
			    $"[IncomingTerminalBlocks], " +
			    $"[IncomingBusbarLinks], " +
			    $"[IncomingFront], " +
			    $"[IncomingRear], " +
			    $"[IncomingLeftRight], " +
			    $"[IncomingTopCables], " +
			    $"[IncomingTopBusduct], " +
			    $"[IncomingBottomCables], " +
			    $"[IncomingScrews], " +
			    $"[IncomingGlandPlate], " +
			    $"[IncomingCableGland], " +
			    $"[IncomingShrouding], " +
			    $"[OutgoingFixed], " +
			    $"[OutgoingPlugIn], " +
			    $"[OutgoingDrawout], " +
			    $"[OutgoingMotorized], " +
			    $"[OutgoingBehindDoor], " +
			    $"[OutgoingThroughPlate], " +
			    $"[OutgoingInterLock], " +
			    $"[OutgoingPadlocking], " +
			    $"[OutgoingShutter], " +
			    $"[OutgoingThroughDoor], " +
			    $"[OutgoingThroughCover], " +
			    $"[OutgoingDirect], " +
			    $"[OutgoingTerminalBlocks], " +
			    $"[OutgoingBusbarLinks], " +
			    $"[OutgoingFront], " +
			    $"[OutgoingRear], " +
			    $"[OutgoingLeftRight], " +
			    $"[OutgoingTopCables], " +
			    $"[OutgoingTopBusduct], " +
			    $"[OutgoingBottomCables], " +
			    $"[OutgoingScrews], " +
			    $"[OutgoingGlandPlate], " +
			    $"[OutgoingCableGland], " +
			    $"[OutgoingShrouding], " +
			    $"[PushButtonON], " +
			    $"[PushButtonOFF], " +
			    $"[PushButtonReset], " +
			    $"[SignallingON], " +
			    $"[SignallingOFF], " +
			    $"[SignallingTrip], " +
			    $"[ExternalLabelType], " +
			    $"[ExternalLabelFixeing], " +
			    $"[ExternalLabelLanguage], " +
			    $"[InternalLabelType], " +
			    $"[InternalLabelFixeing], " +
			    $"[InternalLabelLanguage], " +
			    $"[EquipmentLabelType], " +
			    $"[EquipmentLabelFixeing], " +
			    $"[EquipmentLabelLanguage], " +
			    $"[LabelBackground], " +
			    $"[LabelFont], " +
			    $"[AuxiliaryVoltageSection], " +
			    $"[AuxiliaryVoltageColor], " +
			    $"[AuxiliaryVoltageType], " +
			    $"[AuxiliaryCurrentSection], " +
			    $"[AuxiliaryCurrentColor], " +
			    $"[AuxiliaryCurrentType], " +
			    $"[ApparatusDefind], " +
			    $"[Weight], " +
			    $"[ForInformation], " +
			    $"[ForProduction], " +
			    $"[ForApproval], " +
			    $"[AsManufactured], " +
			    $"[Remarks]) " +

                $"SELECT [PanelID], " +
	            $"[RevisePanelID], " +
	            $"[QuotationID], " +
	            $"[PanelSN], " +
	            $"[PanelName], " +
	            $"[PanelQty], " +
	            $"[PanelType], " +
	            $"[PanelProfit], " +
	            $"[EnclosureName], " +
	            $"[EnclosureType], " +
	            $"[EnclosureLocation], " +
	            $"[EnclosureInstallation], " +
	            $"[EnclosureHeight], " +
	            $"[EnclosureWidth], " +
	            $"[EnclosureDepth], " +
	            $"[EnclosureIP], " +
	            $"[EnclosureColor], " +
	            $"[EnclosureMetalType], " +
	            $"[EnclosureForm], " +
	            $"[EnclosureDoor], " +
	            $"[EnclosureFunctional], " +
	            $"[Source], " +
	            $"[Icu], " +
	            $"[Frequency], " +
	            $"[PowerSupplyOperation], " +
	            $"[EarthingSystem], " +
	            $"[DirectCurrent], " +
	            $"[Busbar], " +
	            $"[BusbarHorizontal], " +
	            $"[BusbarVertical], " +
	            $"[NeutralSize], " +
	            $"[EarthSize], " +
	            $"[SignallingVoltage], " +
	            $"[SignallingSource], " +
	            $"[ControlVoltage], " +
	            $"[ControlSource], " +
	            $"[SensorsVoltage], " +
	            $"[SensorsSource], " +
	            $"[LightingVoltage], " +
	            $"[LightingSource], " +
	            $"[ClimateType], " +
	            $"[AtmosphereType], " +
	            $"[PollutionRisks], " +
	            $"[AmbientTemperature], " +
	            $"[RelativeHumidity], " +
	            $"[SeaLevel], " +
	            $"[IndicationType], " +
	            $"[IndicationSize], " +
	            $"[IndicationOther], " +
	            $"[MeterRelay], " +
	            $"[IncomingFixed], " +
	            $"[IncomingPlugIn], " +
	            $"[IncomingDrawout], " +
	            $"[IncomingMotorized], " +
	            $"[IncomingBehindDoor], " +
	            $"[IncomingThroughPlate], " +
	            $"[IncomingInterLock], " +
	            $"[IncomingPadlocking], " +
	            $"[IncomingShutter], " +
	            $"[IncomingThroughDoor], " +
	            $"[IncomingThroughCover], " +
	            $"[IncomingDirect], " +
	            $"[IncomingTerminalBlocks], " +
	            $"[IncomingBusbarLinks], " +
	            $"[IncomingFront], " +
	            $"[IncomingRear], " +
	            $"[IncomingLeftRight], " +
	            $"[IncomingTopCables], " +
	            $"[IncomingTopBusduct], " +
	            $"[IncomingBottomCables], " +
	            $"[IncomingScrews], " +
	            $"[IncomingGlandPlate], " +
	            $"[IncomingCableGland], " +
	            $"[IncomingShrouding], " +
	            $"[OutgoingFixed], " +
	            $"[OutgoingPlugIn], " +
	            $"[OutgoingDrawout], " +
	            $"[OutgoingMotorized], " +
	            $"[OutgoingBehindDoor], " +
	            $"[OutgoingThroughPlate], " +
	            $"[OutgoingInterLock], " +
	            $"[OutgoingPadlocking], " +
	            $"[OutgoingShutter], " +
	            $"[OutgoingThroughDoor], " +
	            $"[OutgoingThroughCover], " +
	            $"[OutgoingDirect], " +
	            $"[OutgoingTerminalBlocks], " +
	            $"[OutgoingBusbarLinks], " +
	            $"[OutgoingFront], " +
	            $"[OutgoingRear], " +
	            $"[OutgoingLeftRight], " +
	            $"[OutgoingTopCables], " +
	            $"[OutgoingTopBusduct], " +
	            $"[OutgoingBottomCables], " +
	            $"[OutgoingScrews], " +
	            $"[OutgoingGlandPlate], " +
	            $"[OutgoingCableGland], " +
	            $"[OutgoingShrouding], " +
	            $"[PushButtonON], " +
	            $"[PushButtonOFF], " +
	            $"[PushButtonReset], " +
	            $"[SignallingON], " +
	            $"[SignallingOFF], " +
	            $"[SignallingTrip], " +
	            $"[ExternalLabelType], " +
	            $"[ExternalLabelFixeing], " +
	            $"[ExternalLabelLanguage], " +
	            $"[InternalLabelType], " +
	            $"[InternalLabelFixeing], " +
	            $"[InternalLabelLanguage], " +
	            $"[EquipmentLabelType], " +
	            $"[EquipmentLabelFixeing], " +
	            $"[EquipmentLabelLanguage], " +
	            $"[LabelBackground], " +
	            $"[LabelFont], " +
	            $"[AuxiliaryVoltageSection], " +
	            $"[AuxiliaryVoltageColor], " +
	            $"[AuxiliaryVoltageType], " +
	            $"[AuxiliaryCurrentSection], " +
	            $"[AuxiliaryCurrentColor], " +
	            $"[AuxiliaryCurrentType], " +
	            $"[ApparatusDefind], " +
	            $"[Weight], " +
	            $"[ForInformation], " +
	            $"[ForProduction], " +
	            $"[ForApproval], " +
	            $"[AsManufactured], " +
	            $"[Remarks] " +
                $"FROM [Quotation].[QuotationsPanels] " +

				$"ORDER BY [PanelID] ASC;" +
				$"SET IDENTITY_INSERT Quotation._Panels OFF;");

		}

		public override void Down()
        {
			DropTable("Quotation._Panels");
        }
    }
}
