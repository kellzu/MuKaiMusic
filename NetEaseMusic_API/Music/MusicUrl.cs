﻿using System.Collections;

namespace MusicApi.NetEase.Music
{
    /// <summary>
    /// 获取歌曲Url
    /// </summary>
    public class Music_Url : BaseRequestOption
    {
        public Music_Url(Hashtable cookies, int[] ids, int br) : base(cookies)
        {

            this.Params.Add("ids", "[" + string.Join(',', ids) + "]");
            this.Params.Add("br", br);
        }

        public Music_Url(int id, int br) : this(new Hashtable(), new int[] { id }, br)
        {

        }

        public Music_Url(int id) : this(id, 999000)
        {

        }

        public Music_Url(Hashtable cookies, int id) : this(cookies, new int[] { id }, 999000)
        {

        }



        public override string Url => "https://music.163.com/api/song/enhance/player/url";

        public override CryptoType Crypto => CryptoType.Netease_linuxApi;

        public override string Ua { get; }

        public override HttpMethod HttpMethod => HttpMethod.POST;

        public override string OptionUrl { get; }
    }
}
