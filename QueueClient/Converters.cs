using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace QueueClient;

/// <summary>Склеивает список строк в "a, b, c" для отображения ролей и т.п.</summary>
public static class StringJoin
{
    public static readonly IValueConverter Comma = new FuncValueConverter<IEnumerable?, string>(
        items => items is null ? "" : string.Join(", ", items.Cast<object?>().Select(x => x?.ToString())));
}

/// <summary>Тексты кнопок-переключателей в списке услуг.</summary>
public static class Labels
{
    public static readonly IValueConverter ActiveToggle = new FuncValueConverter<bool, string>(
        active => active == true ? "Активна ✅" : "Выключена ⛔");

    public static readonly IValueConverter FacetsToggle = new FuncValueConverter<bool, string>(
        need => need == true ? "Данные: да" : "Данные: нет");
}
