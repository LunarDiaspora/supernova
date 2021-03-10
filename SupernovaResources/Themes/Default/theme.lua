Theme = {
    Name = "Default Theme"
}

local h = SN_LoadImage("Themes/Default/key.png")
local mh = false

function OnUpdate(dt)
end

function OnStart()

end

Theme.NoteYOffset = 150
Theme.NoteXOffset = 250
Theme.NoteWidth = 60
Theme.NoteHeight = 20

local COLOUR_SC = {227, 64, 32, 255}
local COLOUR_WHITE = {242, 242, 242, 255}
local COLOUR_BLUE = {34, 136, 214, 255}

Theme.NoteColours = {
    [0] = COLOUR_SC,
    COLOUR_WHITE, COLOUR_BLUE,
    COLOUR_WHITE, COLOUR_BLUE,
    COLOUR_WHITE, COLOUR_BLUE,
    COLOUR_WHITE
}

function OnDraw()
    local gp = SN_GetGameplay()

    --SN_SetDrawColour(255, 0, 0, 255)
    --SN_DrawFilledRect(100, 100, 100, 100)

    --SN_SetDrawColour(255, 255, 255, 255)
    --SN_SetFont("standard")
    --SN_DrawText("poggers", 300, 300)

    SN_SetDrawColour(214, 15, 15, 128)
    SN_DrawFilledRect(Theme.NoteXOffset,(720-Theme.NoteYOffset),8*Theme.NoteWidth,Theme.NoteHeight)

    if gp.Started then
        SN_SetDrawColour(255, 255, 255, 255)
        SN_SetFont("monospace")
        local title = gp.Chart.artist .. " - " .. gp.Chart.title
        if gp.Chart.subtitle then
            title = title .. " " .. gp.Chart.subtitle
        end
        SN_DrawText(title, Theme.NoteXOffset, (720-Theme.NoteYOffset)+30)
    end

    --if gp.Started then
    --    for k=0,gp.Notes.Count-1 do
    --        local v = gp.Notes[k]
    --    end
    --end

    --h.Draw(300, 150)
end

--function OnChartLoad()
--    notes = SN_GetNotes()
--end

--[[
function DrawNote(v)
    local gp = SN_GetGameplay()

    if v.Column then
        local cl = cols[v.Column]
        if cl then
            local CalculatedY = (720-offset)-((v.Beat-gp.Beat)*nh*(12))
            if CalculatedY > -nh then
                SN_SetDrawColour(cl[1], cl[2], cl[3], 255)
                SN_DrawFilledRect((nw*v.Column)+xoffset, CalculatedY, nw, nh)
            end
        end
    end
end
]]