// Having trouble with circular dependency
// const SteamClient = require('../steamClient')

class ISteamUser {

    interface = "ISteamUser"

    // http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key=XXXXXXXXXXXXXXXXXXXXXX&format=json&vanityurl=thekronn0s
    // {"response":{"steamid":"76561198132808518","success":1}}
    async resolveVanityUrl(steamClient, vanityUrl, format = 'json') {
        const version = 'v0001'
        const method = 'ResolveVanityURL'

        // if (!(steamClient instanceof SteamClient)) {
        //     throw new Error('steamClient is not a instance of SteamClient')
        // }

        const builder = steamClient.createUrlBuilder(
            this.interface,
            method,
            version,
            format
        )

        builder.addQuery('vanityurl', vanityUrl)

        try {
            return await steamClient.getAsync(builder.build())
        } catch (error) {
            throw error
        }
    }

    async getPlayerBans(steamClient, steamids, format = 'json') {
        const { version, method } = steamClient.getVersionAndMethod('v0001', 'GetPlayerBans')

        const builder = steamClient.createUrlBuilder(
            this.interface,
            method,
            version,
            format
        )

        builder.addQuery('steamids', steamids)

        try {
            return await steamClient.getAsync(builder.build())
        } catch (error) {
            throw error
        }
    }
    
    async getPlayerSummaries(steamClient, steamIds, format = 'json') {
        const version = 'v0002'
        const method = 'GetPlayerSummaries'

        // if (!(steamClient instanceof SteamClient)) {
        //     throw new Error('steamClient is not a instance of SteamClient')
        // }

        // let request = `https://${steamClient.base}/${this.interface}/GetPlayerSummaries/${version}/?key=${steamClient.key}&steamids=`
        let builder = steamClient.createUrlBuilder(this.interface, method, version, format)
        
        if (Array.isArray(steamIds)) {
            // let ids = ''
            
            // steamIds.forEach((id, i) => {
            //     if (i !== 0) {
            //         ids += id + ','
            //     } else {
            //         ids += id
            //     }
            // });
            
            const ids = steamIds.reduce((current, next) => current + ',' + next, '')

            builder.addQuery('steamids', ids)
        } else {
            builder.addQuery('steamids', steamIds)
        }

        try {
            return await steamClient.getAsync(builder.build())
        } catch (error) {
            throw error
        }
    }

}

module.exports = ISteamUser