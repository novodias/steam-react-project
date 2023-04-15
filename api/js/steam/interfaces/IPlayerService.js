// const SteamClient = require('../steamClient')

class IPlayerService {

    interface = 'IPlayerService'

    async getRecentlyPlayedGames(steamClient, steamid, count, format = 'json') {
        const version = 'v0001'
        const method = 'GetRecentlyPlayedGames'

        // if (!(steamClient instanceof SteamClient)) {
        //     throw new Error('steamClient is not a instance of SteamClient')
        // }

        const builder = steamClient.createUrlBuilder(
            this.interface,
            method, 
            version,
            format
        )
        
        // builder.addQuery('steamid', steamid)
        // builder.addQuery('count', count)
        builder.setJson('steamid', steamid)
        builder.setJson('count', count)

        try {
            return await steamClient.getAsync(builder.build())
        } catch (error) {
            throw error
        }
    }
}

module.exports = IPlayerService