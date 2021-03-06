Theme = {
    Name = "Default Theme"
}

--LuaTest()
local obj = SN_AddRectangle(100, 100)
print(obj)
print(obj.Position)

obj.Position.X = 50

function OnUpdate(dt)
    obj.Position.X = obj.Position.X + (dt * 20)
end

function OnStart()
    print("OnStart")
    print(obj.Position.X)
end