using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    protected override void Init()
    {
        base.Init();

        // 초기화 시 챕터나 아이템 데이터를 로드하지 않음
    }
}
