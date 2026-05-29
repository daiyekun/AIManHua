import { create } from "zustand";
import type { ComicTask, Storyboard, GeneratedImage } from "../types";

interface ComicState {
  currentTask: ComicTask | null;
  storyboards: Storyboard[];
  images: GeneratedImage[];
  setCurrentTask: (task: ComicTask | null) => void;
  setStoryboards: (storyboards: Storyboard[]) => void;
  setImages: (images: GeneratedImage[]) => void;
}

export const useComicStore = create<ComicState>((set) => ({
  currentTask: null,
  storyboards: [],
  images: [],
  setCurrentTask: (task) => set({ currentTask: task }),
  setStoryboards: (storyboards) => set({ storyboards }),
  setImages: (images) => set({ images }),
}));
