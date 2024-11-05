using Microsoft.AspNetCore.Components;

namespace Client_App.Components.Share.Reusable;

public partial class Pagination : ComponentBase
{
    private List<int> PaginationList { get; set; } = null!;
    private int CurrentPage { get; set; } = 1;
    private int PreviousTotalPages { get; set; } = 0;

    [Parameter]
    public int TotalPages { get; set; }

    [Parameter]
    public EventCallback<int> OnPageClick { get; set; }

    protected override void OnParametersSet()
    {
        if (TotalPages == PreviousTotalPages)
            return;

        CurrentPage = 1;
        PreviousTotalPages = TotalPages;

        if (OnPageClick.HasDelegate)
            OnPageClick.InvokeAsync(CurrentPage);

        PaginationList = GetPagination(CurrentPage, TotalPages);
    }

    private List<int> GetPagination(int currentPage, int totalPages)
    {
        List<int> pagination = [];

        for (var i = 1; i <= Math.Min(3, totalPages); i++)
            pagination.Add(i);

        if (currentPage > 4)
            pagination.Add(-1); // Use -1 to represent the "..." placeholder

        var start = Math.Max(4, currentPage - 1);
        var end = Math.Min(currentPage + 1, totalPages - 3);

        for (var i = start; i <= end; i++)
            pagination.Add(i);

        if (currentPage < totalPages - 3)
            pagination.Add(-1); // Use -1 for the "..." placeholder

        for (var i = Math.Max(totalPages - 2, 4); i <= totalPages; i++)
            pagination.Add(i);

        return pagination;
    }

    private void OnPageChangeAsync(int newPage)
    {
        if (newPage < 1 || newPage > TotalPages || newPage == CurrentPage)
            return;

        CurrentPage = newPage;

        PaginationList = GetPagination(CurrentPage, TotalPages);

        if (OnPageClick.HasDelegate)
            OnPageClick.InvokeAsync(CurrentPage);
    }
}
