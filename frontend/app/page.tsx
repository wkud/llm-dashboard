'use client';

import { useState, useEffect, useCallback } from 'react';
import { Box, Container } from '@mui/material';
import { Prompt } from './lib/types/prompt';
import { apiService } from './lib/services/api';
import Header from './components/Header';
import NewPrompt from './components/NewPrompt';
import PromptList from './components/PromptList';

const POLLING_INTERVAL = 3000; // 3 seconds

export default function Home() {
  const [prompts, setPrompts] = useState<Prompt[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  const fetchPrompts = useCallback(async () => {
    try {
      const fetchedPrompts = await apiService.getAllPrompts();
      setPrompts(fetchedPrompts);
    } catch (error) {
      console.error('Failed to fetch prompts:', error);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const handleSubmitPrompt = useCallback(async (text: string) => {
    setIsSubmitting(true);
    try {
      await apiService.createPrompt({ text });
      await fetchPrompts();
    } catch (error) {
      console.error('Failed to submit prompt:', error);
      throw error;
    } finally {
      setIsSubmitting(false);
    }
  }, [fetchPrompts]);

  const handleDeletePrompt = useCallback(async (id: string) => {
    try {
      await apiService.deletePrompt(id);
      await fetchPrompts();
    } catch (error) {
      console.error('Failed to delete prompt:', error);
      throw error;
    }
  }, [fetchPrompts]);

  const handleRefresh = useCallback(() => {
    fetchPrompts();
  }, [fetchPrompts]);

  useEffect(() => {
    fetchPrompts();
  }, [fetchPrompts]);

  useEffect(() => {
    const interval = setInterval(() => {
      fetchPrompts();
    }, POLLING_INTERVAL);

    return () => clearInterval(interval);
  }, [fetchPrompts]);

  return (
    <Box
      sx={{
        minHeight: '100vh',
        backgroundColor: '#0f172a',
        backgroundImage: 'radial-gradient(at 0% 0%, rgba(59, 130, 246, 0.05) 0px, transparent 50%), radial-gradient(at 100% 100%, rgba(16, 185, 129, 0.05) 0px, transparent 50%)',
      }}
    >
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Header promptCount={prompts.length} onRefresh={handleRefresh} />
        <NewPrompt onSubmit={handleSubmitPrompt} isSubmitting={isSubmitting} />
        {!isLoading && (
          <PromptList prompts={prompts} onDelete={handleDeletePrompt} />
        )}
      </Container>
    </Box>
  );
}
