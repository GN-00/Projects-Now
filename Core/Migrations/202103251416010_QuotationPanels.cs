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
					Id = c.Int(nullable:false, identity: true),
			RevisePanelId
			QuotationId
			SN
			Name
			Qty
			Type
			Profit
			EnclosureName
			EnclosureType
			EnclosureInstallation
			EnclosureHeight
			EnclosureWidth
			EnclosureDepth
			EnclosureIP
			EnclosureLocation
			EnclosureColor
			EnclosureMetalType
			EnclosureForm
			EnclosureDoor
			EnclosureFunctional


			Source
			Icu
			Frequency = "60Hz";
			PowerSupplyOperation = "No Parallel Operation";
			EarthingSystem = "Earthed (TNS)";
			DirectCurrent = "To Earth";

			Busbar = "Bare Copper";
			BusbarHorizontal
			BusbarVertical
			NeutralSize
			EarthSize

			SignallingVoltage = "N/A";
			SignallingSource = "N/A";
			ControlVoltage = "N/A";
			ControlSource = "N/A";
			SensorsVoltage = "N/A";
			SensorsSource = "N/A";
			LightingVoltage = "N/A";
			LightingSource = "N/A";

			ClimateType = "Normal";
			AtmosphereType = "Ordinary";
			PollutionRisks
			AmbientTemperature = 40;
			RelativeHumidity = 50;
			SeaLevel = 0;

			IndicationType = "Without";
			IndicationSize
			IndicationOther
			MeterRelay

			IncomingFixed = false;
			IncomingPlugIn = false;
			IncomingDrawout = false;
			IncomingMotorized = false;
			IncomingBehindDoor = false;
			IncomingThroughPlate = false;
			IncomingInterLock = false;
			IncomingPadlocking = false;
			IncomingShutter = false;
			IncomingThroughDoor = false;
			IncomingThroughCover = false;
			IncomingDirect = false;
			IncomingTerminalBlocks = false;
			IncomingBusbarLinks = false;
			IncomingFront = false;
			IncomingRear = false;
			IncomingLeftRight = false;
			IncomingTopCables = false;
			IncomingTopBusduct = false;
			IncomingBottomCables = false;
			IncomingScrews = false;
			IncomingGlandPlate = false;
			IncomingCableGland = false;
			IncomingShrouding = false;

			OutgoingFixed = false;
			OutgoingPlugIn = false;
			OutgoingDrawout = false;
			OutgoingMotorized = false;
			OutgoingBehindDoor = false;
			OutgoingThroughPlate = false;
			OutgoingInterLock = false;
			OutgoingPadlocking = false;
			OutgoingShutter = false;
			OutgoingThroughDoor = false;
			OutgoingThroughCover = false;
			OutgoingDirect = false;
			OutgoingTerminalBlocks = false;
			OutgoingBusbarLinks = false;
			OutgoingFront = false;
			OutgoingRear = false;
			OutgoingLeftRight = false;
			OutgoingTopCables = false;
			OutgoingTopBusduct = false;
			OutgoingBottomCables = false;
			OutgoingScrews = false;
			OutgoingGlandPlate = false;
			OutgoingCableGland = false;
			OutgoingShrouding = false;

			PushButtonON = "Without";
			PushButtonOFF = "Without";
			PushButtonReset = "Without";
			SignallingON = "Without";
			SignallingOFF = "Without";
			SignallingTrip = "Without";

			ExternalLabelType = "Gravely";
			ExternalLabelFixeing = "Glued";
			ExternalLabelLanguage = "English";
			InternalLabelType = "Gravely";
			InternalLabelFixeing = "Glued";
			InternalLabelLanguage = "English";
			EquipmentLabelType = "Sticker";
			EquipmentLabelFixeing = "Self Stick";
			EquipmentLabelLanguage = "English";
			LabelBackground = "Black";
			LabelFont = "White";

			AuxiliaryVoltageSection = 1.5;
			AuxiliaryVoltageColor = "Black";
			AuxiliaryVoltageType = "XLPE";
			AuxiliaryCurrentSection = 1.5;
			AuxiliaryCurrentColor = "Black";
			AuxiliaryCurrentType = "XLPE";

			ApparatusDefind = "Yes";
			Weight = "XX";
			ForInformation
			ForProduction
			ForApproval
			AsManufactured
			Remarks
				});

	}
        
        public override void Down()
        {
        }
    }
}
