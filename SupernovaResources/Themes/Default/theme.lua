Theme = {
    Name = "Default Theme"
}

local h = SN_LoadImage("Themes/Default/key.png")

function OnUpdate(dt)
    local g = SN_GetGameplay()
    --print(g.Beat)
end

function OnStart()

end

function OnDraw()
    local gp = SN_GetGameplay()

    SN_SetDrawColour(255, 0, 0, 255)
    SN_DrawFilledRect(100, 100, 100, 100)

    SN_SetDrawColour(255, 255, 255, 255)
    SN_SetFont("standard")
    SN_DrawText("poggers", 300, 300)

    h.Draw(300, 150)
end

--function OnChartLoad()

--end