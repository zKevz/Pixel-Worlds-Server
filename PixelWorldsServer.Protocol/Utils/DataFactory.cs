using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Worlds;

namespace PixelWorldsServer.Protocol.Utils;

public static class DataFactory
{
    public static Type GetDataTypeForEnum(BlockType bt)
    {
        return bt switch
        {
            BlockType.LockSmall => typeof(LockSmallData),
            BlockType.EntrancePortal => typeof(EntrancePortalData),
            BlockType.LanternBlue => typeof(LanternBlueData),
            BlockType.LabHoseLarge => typeof(LabHoseLargeData),
            BlockType.LabLightRed => typeof(LabLightRedData),
            BlockType.SewerPipeBlack => typeof(SewerPipeBlackData),
            BlockType.TutorialSleepPod => typeof(TutorialSleepPodData),
            BlockType.TutorialCablePortal => typeof(TutorialCablePortalData),
            BlockType.Stereos => typeof(StereosData),
            BlockType.ScifiArrow => typeof(ScifiArrowData),
            BlockType.Portal => typeof(PortalData),
            BlockType.BattleBarrierLab => typeof(BattleBarrierLabData),
            BlockType.ScifiLights => typeof(ScifiLightsData),
            BlockType.ScifiDoor => typeof(ScifiDoorData),
            BlockType.LabElectricWireBlue => typeof(LabElectricWireBlueData),
            BlockType.ScifiComputer => typeof(ScifiComputerData),
            BlockType.LabElectricWireRed => typeof(LabElectricWireRedData),

            _ => throw new InvalidDataException($"Unrecognized data {Enum.GetName(bt)}")
        };
    }

    public static WorldItemBase SpawnDataClassForEnum(BlockType bt)
    {
        return SpawnDataClassForEnum(bt, 0);
    }

    public static WorldItemBase SpawnDataClassForEnum(BlockType bt, int itemId = 0)
    {
        return bt switch
        {
            BlockType.LockSmall => new LockSmallData(itemId),
            BlockType.EntrancePortal => new EntrancePortalData(itemId),
            BlockType.LanternBlue => new LanternBlueData(itemId),
            BlockType.LabHoseLarge => new LabHoseLargeData(itemId),
            BlockType.LabLightRed => new LabLightRedData(itemId),
            BlockType.SewerPipeBlack => new SewerPipeBlackData(itemId),
            BlockType.TutorialSleepPod => new TutorialSleepPodData(itemId),
            BlockType.TutorialCablePortal => new TutorialCablePortalData(itemId),
            BlockType.Stereos => new StereosData(itemId),
            BlockType.ScifiArrow => new ScifiArrowData(itemId),
            BlockType.Portal => new PortalData(itemId),
            BlockType.BattleBarrierLab => new BattleBarrierLabData(itemId),
            BlockType.ScifiLights => new ScifiLightsData(itemId),
            BlockType.ScifiDoor => new ScifiDoorData(itemId),
            BlockType.LabElectricWireBlue => new LabElectricWireBlueData(itemId),
            BlockType.ScifiComputer => new ScifiComputerData(itemId),
            BlockType.LabElectricWireRed => new LabElectricWireRedData(itemId),

            _ => throw new InvalidDataException($"Unrecognized data {Enum.GetName(bt)}")
        };
    }
}
