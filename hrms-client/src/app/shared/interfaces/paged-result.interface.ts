// Common paginated result structure from backend
export interface PagedResult<T> {
  items: T[];
  meta: PaginationMeta;
}

export interface PaginationMeta {
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
