Theme = {
    Name = "Default Song Select"
}

local folders = SN_GetSongs()

local state = 0
local selectables = {}
local index = 0

local textOffset = 30
local distance = 5
local textVertical = 40

local function findFolder(name: string)
    local f = nil
    foreach k in folders do
        if k.Name == name then
            f = k
        end
    end
    return f
end

function OnStart()
    foreach folder in folders do
        table.insert(selectables, folder.Name)
    end

    state = 0 -- "choose folder"
end

function OnUpdate(dt: float)

end

function OnDraw()
    SN_SetFont("monospace")
    SN_DrawText("song select", 0, 0)
    for i, j in ipairs(selectables) do
        local t = SN_GetTextDimensions(j)
        SN_DrawText(j, textOffset, ((t.h + distance) * i) + textVertical)
    end
end