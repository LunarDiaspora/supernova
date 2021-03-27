Theme = {
    Name = "Default Song Select"
}

local folders = SN_GetSongs()

local state = 0
local selectables = {}
local index = 1

local textOffset = 30
local distance = 5
local textVertical = 40

local selectedFolder = nil

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
        table.insert(selectables, {name = folder.Name, obj = folder})
    end

    state = 0 -- "choose folder"
end

function OnUpdate(dt: float)

end

function OnDraw()
    SN_SetDrawColour(255,255,255)
    SN_SetFont("monospace")
    SN_DrawText("song select", 0, 0)
    for i, j in ipairs(selectables) do
        local t = SN_GetTextDimensions(j.name)
        if i == index then
            SN_SetDrawColour(200, 30, 30)
        else
            SN_SetDrawColour(255,255,255)
        end
        SN_DrawText(j.name, textOffset, ((t.h + distance) * i) + textVertical)
    end
end

function OnKeyDown(s)
    if s == "SDL_SCANCODE_UP" then
        index = index - 1
        if index <= 0 then
            index = #selectables
        end
    end

    if s == "SDL_SCANCODE_DOWN" then
        index = index + 1
        if index > #selectables then
            index = 1
        end
    end

    --Log.Debug(s)

    if s == "SDL_SCANCODE_RETURN" then
        Log.Debug("default select: advancing...")
        if state == 0 then
            selectedFolder = selectables[index].obj
            state = 1
        end
    end
end