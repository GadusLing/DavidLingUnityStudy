GameObject = CS.UnityEngine.GameObject
UI = CS.UnityEngine.UI

local slider = GameObject.Find("Slider")
print(slider)
local sliderScript = slider:GetComponent(typeof(UI.Slider))
print(sliderScript)
sliderScript.onValueChanged:AddListener(function(value)
    print("Slider的值被改变了 新值为:", value)
end)