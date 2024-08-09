//-----------------------------------------------------------------------
// <summary>
// 文件名: LocaltionSO
// 描述: 游戏关卡场景
// 作者: #AUTHOR#
// 创建日期: #CREATIONDATE#
// 修改记录: #MODIFICATIONHISTORY#
// </summary>
//-----------------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewLocation", menuName = "Scene Data/Location")]
public class LocationSO : GameSceneSO
{
	public LocalizedString locationName;
}
