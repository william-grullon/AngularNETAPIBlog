export interface BlogPost {
  id: string;
  title: string;
  shortDescription: string;
  content: string;
  contentHtml?: string;
  featureImageUrl: string;
  urlHandle: string;
  publishedDate: string;
  author: string;
  isVisible: boolean;
}
