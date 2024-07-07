[【Unity教程搬运】使用UI Builder、GraphView和脚本化对象创建行为树](https://www.bilibili.com/video/BV1Yg4y1M7VX/?spm_id_from=333.788&vd_source=56c4342823eb8458689563e7f2be4f99)

[【Unity教程搬运】使用UI Builder的行为树编辑器](https://www.bilibili.com/video/BV1484y1T7gT/?spm_id_from=333.788&vd_source=56c4342823eb8458689563e7f2be4f99)

up使用的是`unity2021.1.10f1`版本，而我使用的是`unity2022.3.17f1c1`版本，unity引擎有些许不一样的地方，如调用执行的顺序、接口等，一下是记录与up视频的不同点，方便后续复盘

### `BehaviourTreeEditor.cs`

#### `OnOpenAsset`

这个方法的作用是在双击Project中的BehaviourTree文件时打开BehaviourTreeEditor窗口



这个接口的参数变了，但其实也并没有用到这个参数，所以没有任何影响

原版

```C#
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId int line)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }
```

修改版

```C#
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }
```





#### `OnPlayModeStateChanged`

这个方法是为了防止程序崩溃：在退出PlayMode时，依然显示克隆版本的行为树视图，如果鼠标点击克隆的节点元素，程序会崩溃。

实现效果是，在退出playmode后，重新描绘行为树视图，显示为最初版的行为树



与up不一样的地方是我注释了进入playmode时的触发事件，因为在进入playmode时，系统会自动刷新一遍BehaviourTreeEditor窗口，所以不用调用

原版

```C#
        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }
```

修改版

```c#
        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    // 在进入playmode时，系统会自动刷新一遍BehaviourTreeEditor窗口，所以不用调用
                    //OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }
```

