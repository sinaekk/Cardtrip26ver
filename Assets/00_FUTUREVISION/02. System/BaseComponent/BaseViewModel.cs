/*
 * DATE     : 2024.11.27
 * AUTHOR   : Kim Bum Moo
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FUTUREVISION
{
    public class BaseViewModel : Base
    {
        [Header("Base View Model")]
        public List<BaseView> SubViewList;
        public override void Initialize()
        {
            base.Initialize();

            SubViewList.ForEach(view =>
            {
                if (view == null)
                {
                    Debug.LogWarning($"[{GetType().Name}] SubViewList에 null인 View가 있습니다. Inspector에서 확인해주세요.", this);
                    return;
                }
                view.Initialize();
            });
        }
    }
}