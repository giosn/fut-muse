namespace FutMuse.API.Extensions
{
    public static class ReplaceEntityExtension
    {
        static readonly Dictionary<string, string> entityList = new()
        {
            { "Italienischer Zweitligameister", "Serie B Champion" },
            { "Uefa", "UEFA" },
            { "Europapokal der Pokalsieger Sieger", "UEFA Cup Winners' Cup" },
            { "Under-17", "U17" },
            { "Under-19", "U19" },
            { "Under-20", "U20" },
            { "Under-21", "U21" },
            { "U20-Weltmeisterschaft", "U20 World Cup" }
        };

        public static string ReplaceEntity(this string entity)
        {
            var newEntity = entityList.FirstOrDefault(e => entity.ToLower().Contains(e.Key.ToLower()));

            if (newEntity.Key is not null)
            {
                entity = entity.Replace(newEntity.Key, newEntity.Value);
            }

            return entity;
        }
    }
}

