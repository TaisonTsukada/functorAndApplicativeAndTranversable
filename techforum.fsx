#r "nuget: FSharpPlus, 1.6.1"
open FSharpPlus
type CompanyId = CompanyId of string
type CompanyListId = CompanyListId of string
type DashboardAuthor = DashboardAuthor of string
type CompanyListIds = CompanyListIds of CompanyListId list
type DashboardAuthors = DashboardAuthors of DashboardAuthor list
type RecipientId = RecipientId of string
type RecipientIds = RecipientIds of RecipientId list

module RecipientIds =
    
    
    let getRecipientIds
        (fetchCompanyListId: CompanyId -> Async<CompanyListId>)
        (fetchDashboardAuthor: CompanyListId -> Async<DashboardAuthor>)
        (companyIds: CompanyId list)
        : Async<RecipientIds> =
        async {
            let! companyListIds =
              companyIds
              |> List.map fetchCompanyListId
              
            let! dashboardAuthors =
              companyListIds
              |> List.map fetchDashboardAuthor

            return RecipientIds(dashboardAuthors |> List.map (fun (DashboardAuthor (x: string)) -> RecipientId x))
        }
    


let getRecipientIds
  (fetchCompanyListId: CompanyId -> Async<CompanyListId>)
  (fetchDashboardAuthor: CompanyListId -> Async<DashboardAuthor>)
  (companyIds: CompanyId list)
  : Async<RecipientIds> =
  async {
    let! companyListIds = traverse fetchCompanyListId companyIds
    let! dashboardAuthors = traverse fetchDashboardAuthor companyListIds
    return RecipientIds(dashboardAuthors |> List.map (fun (DashboardAuthor x) -> RecipientId x))
  }


let getRecipientIds
  (fetchCompanyListIds: CompanyId -> Async<CompanyListIds>)
  (fetchDashboardAuthors: CompanyListId -> Async<DashboardAuthors>)
  (companyIds: CompanyId list)
  : Async<RecipientIds> =
  async {
      let companyListIds =
        companyIds
        |> List.map fetchCompanyListIds
        (*ここでList<Async<CompanyListIds>>をAsync<List<CompanyListId>>にしたい*)
        
      let! dashboardAuthors =
        companyListIds
        |> List.map fetchDashboardAuthors

      return RecipientIds(dashboardAuthors |> List.map (fun (DashboardAuthor (x: string)) -> RecipientId x))
  }