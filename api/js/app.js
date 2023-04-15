// const fs = require('fs').promises
const express = require('express')
const app = express()
const port = Number.parseInt(process.env.SERVERPORT)
const cors = require('cors')
app.use(cors())

// const Inventory = require('./steam/scraper/Inventory')
const SteamClient = require('./steam/steamClient')
const key = process.env.STEAMAPIKEY
const client = new SteamClient(key)

if (!port) {
    throw new Error('Port was not defined')
}

const SteamMongoDatabase = require('./database/mongoClient')
const steamContext = new SteamMongoDatabase()
steamContext.createCollections()
// steamContext.run()

function isNumeric(value) {
    return /^\d+$/.test(value);
}

app.get('/', (req, res) => {
    res.send('Ok')
})

app.get('/user/:steamid', async (req, res) => {
    const get = async (id) => {
        try {
            const data = await client.getPlayerSummaries(id)
            res.send(data)
            return data
        } catch (error) {
            console.log(error)
            res.send({error: 'User not found'})
        }
    }

    const id = req.params.steamid
    let result = null
    let isvanity = !isNumeric(id)

    if (isvanity) {
        result = await steamContext.findOneUser(null, id)
    } else {
        if (id.toString().length !== 17) {
            res.status(406).send({error: "SteamID is not valid"})
            return
        }
        
        result = await steamContext.findOneUser(id)
    }

    if (result !== null) {
        console.log("[Database][SteamUsers] Found user " + result.steamid)
        const time = Math.abs(new Date() - new Date(result.lastupdate)) / 60000
        
        if (time < 120) {
            res.send(result)
            return
        }
        
        console.log(`[Database][SteamUsers] ${time} minutes finished, updating user: ${result.steamid}`)
        const update = await get(id)
        // console.log(update)
        await steamContext.updateUser(update, update.steamid)
        return
    }
    
    console.log("[Database][SteamUsers] User not found, requesting Steam API...")

    try {
        const user = await get(id)
        await steamContext.insertUser(user)
    } catch (error) {
        console.log(error)
    }
})

app.get('/user/:steamid/recent', (req, res) => {
    const id = req.params.steamid
    const count = req.query.count ? parseInt(req.query.count) : 5
    client.getRecentlyPlayedGames(id, count)
        // .then((response) => {
        //     return response.json()
        // })
        .then((data) => {
            // console.log(data)
            res.send(data)
        })
        .catch((reason) => {
            throw reason
        })
})

app.get('/user/:steamid/bans', (req, res) => {
    const id = req.params.steamid
    client.getPlayerBans(id)
        .then((data) => {
            res.send(data)
        })
        .catch((reason) => {
            throw reason
        })
})

app.get('/user/:steamid/csgo-inventory', async (req, res) => {
    
    const steamid = req.params.steamid

    if (steamid.toString().length !== 17) {
        res.status(406).send({error: "SteamID is not valid"})
        return
    }

    const result = await steamContext.findOneInventory(steamid)

    if (result !== null) {
        console.log("[Database][SteamUsersInventory] Found user inventory " + result._id)
        const time = Math.abs(new Date() - new Date(result.lastupdate)) / 60000

        // 8 hours to update
        if (time < 480) {
            res.send(result)
            return
        }

        console.log(`[Database][SteamUsersInventory] ${time} minutes finished, updating user inventory: ${result._id}`)
        try {
            const update = await client.getPlayerInventory(steamid, 730, 5000)
            await steamContext.updateInventory(update, steamid)
            res.send(update)
        } catch (error) {
            throw error
        }
        
        return
    }

    console.log("[Database][SteamUsersInventory] User inventory not found, requesting Steam API...")
    const userInv = await client.getPlayerInventory(steamid, 730, 5000)
    const insert = await steamContext.insertInventory(userInv, steamid)
    res.send(insert)
    
    // TEST
    // const buffer = await fs.readFile('./inventoryScrapeExample.json')
    // res.send(buffer.toString())
})

app.listen(port, () => {
    console.log('Server listening on port: ' + port)
})