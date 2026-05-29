export interface ComicTask {
  id: number;
  title: string;
  prompt: string;
  style: string;
  status: "pending" | "queued" | "processing" | "completed" | "failed" | "cancelled";
  createdAt: string;
  completedAt?: string;
}

export interface Storyboard {
  id: number;
  comicTaskId: number;
  panelIndex: number;
  sceneDescription: string;
  dialogue: string;
  layoutType: "full" | "split_h" | "split_v" | "grid_2x2";
  x: number;
  y: number;
  width: number;
  height: number;
}

export interface GeneratedImage {
  id: number;
  comicTaskId: number;
  imageUrl: string;
  width: number;
  height: number;
  createdAt: string;
}
