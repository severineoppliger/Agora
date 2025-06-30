export interface PostSummaryDto {
  id: number;
  title: string;
  price: number;
  typeName: string; // 'Offer' or 'Request'
  statusName: string; // 'Active', 'Inactive' or 'Deleted'
  postCategoryName: string;
  userName: string;
}

export interface PostsApiResponse {
  data: PostSummaryDto[];
}