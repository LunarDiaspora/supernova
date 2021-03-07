Theme = {
    Name = "Default Theme"
}

function OnUpdate(dt)
    local g = SN_GetGameplay()
    --print(g.Beat)
end

function OnStart()

end

function OnDraw()
    local h = SN_GetGameplay()

    SN_SetDrawColour(255, 0, 0, 255)
    SN_DrawFilledRect(100, 100, 100, 100)

    SN_SetFont("standard")
    SN_DrawText("poggers", 300, 300)
end

function OnChartLoad()
    local ch = SN_GetNotes()
    print(ch)
end