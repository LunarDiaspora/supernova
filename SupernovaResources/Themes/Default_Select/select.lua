Theme = {
    Name = "Default Song Select"
}

function OnStart()
    local s = SN_GetSongs()
end

function OnUpdate(dt)

end

function OnDraw()
    SN_SetFont("monospace")
    SN_DrawText("song select", 0, 0)
end