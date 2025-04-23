export default interface IPagedResult<T> {
  content: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}
