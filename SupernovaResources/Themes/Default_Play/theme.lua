local COLOUR_SC = {r = 227, g = 64, b = 32, a = 255}
local COLOUR_WHITE = {r = 242, g = 242, b = 242, a = 255}
local COLOUR_BLUE = {r = 34, g = 136, b = 214, a = 255}

Theme = {
    Name = "Default Play Theme",
    Style = "Play",
    NoteColours = {
        COLOUR_SC,
        COLOUR_WHITE, COLOUR_BLUE,
        COLOUR_WHITE, COLOUR_BLUE,
        COLOUR_WHITE, COLOUR_BLUE,
        COLOUR_WHITE
    },
    NoteXOffset = 150,
    NoteYOffset = 250,
    NoteWidth = 60,
    NoteHeight = 20
}

--local h = SN_LoadImage("Themes/Default_Play/key.png")
local mh = false

local judgeTimer = 0
local judgeString = ""

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
        local diffText = tostring(gp.Chart.difficulty) .. " Lv. " .. (gp.Chart.playLevel or "???")
        SN_DrawText(title, Theme.NoteXOffset, (720-Theme.NoteYOffset)+30)
        SN_DrawText(diffText, Theme.NoteXOffset, (720-Theme.NoteYOffset)+65)

        local infoText = "EX-Score: "..tostring(gp.EXScore)
        SN_DrawText(infoText, Theme.NoteXOffset, (720-Theme.NoteYOffset)+100)

    end

    --if gp.Started then
    --    for k=0,gp.Notes.Count-1 do
    --        local v = gp.Notes[k]
    --    end
    --end

    --h.Draw(300, 150)
end

function DrawAfterNotes()
    if judgeTimer < 1 then
        SN_SetFont("monospace")
        local dim = SN_GetTextDimensions(judgeString)
        local totalWidth = Theme.NoteWidth * 8
        local half = totalWidth / 2
        local x = half - (dim.w / 2)
        SN_DrawText(judgeString, Theme.NoteXOffset + x, (720-Theme.NoteYOffset)-100)
    end
end

function OnUpdate(dt)
    judgeTimer = judgeTimer + dt
end

local judgestrings = {
    PERFECT_GREAT = "Great!",
    GREAT = "Great",
    GOOD = "Good",
    BAD = "Bad",
    POOR = "Poor",
    EXTRA_POOR = "Poor"
}

function OnJudgement(j)
    local g = Global.Gameplay
    local m = judgestrings[tostring(j)]
    if g.Combo >= 1 then
        m = m .. " " .. tostring(g.Combo)
    end
    judgeString = m
    judgeTimer = 0
end

function OnStart()

end

function OnChartLoad()

end