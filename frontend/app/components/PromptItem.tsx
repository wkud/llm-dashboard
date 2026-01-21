'use client';

import {
  Box,
  Typography,
  Paper,
  Chip,
  IconButton,
  Tooltip,
  Collapse,
} from '@mui/material';
import {
  Delete as DeleteIcon,
  ExpandMore as ExpandMoreIcon,
  ExpandLess as ExpandLessIcon,
} from '@mui/icons-material';
import { Prompt, PromptStatus } from '../lib/types/prompt';
import { useState } from 'react';

interface PromptItemProps {
  prompt: Prompt;
  onDelete: (id: string) => Promise<void>;
}

const getStatusColor = (status: PromptStatus): 'default' | 'primary' | 'success' | 'error' | 'warning' => {
  switch (status) {
    case PromptStatus.Pending:
      return 'primary'; // Blue
    case PromptStatus.Processing:
      return 'warning'; // Amber
    case PromptStatus.Completed:
      return 'success'; // Green
    case PromptStatus.Failed:
      return 'error'; // Red
    default:
      return 'default';
  }
};

const getStatusChipStyles = (status: PromptStatus) => {
  switch (status) {
    case PromptStatus.Pending:
      return {
        backgroundColor: 'rgba(59, 130, 246, 0.15)',
        color: '#3b82f6',
        border: '1px solid rgba(59, 130, 246, 0.3)',
      };
    case PromptStatus.Processing:
      return {
        backgroundColor: 'rgba(245, 158, 11, 0.15)',
        color: '#f59e0b',
        border: '1px solid rgba(245, 158, 11, 0.3)',
      };
    case PromptStatus.Completed:
      return {
        backgroundColor: 'rgba(16, 185, 129, 0.15)',
        color: '#10b981',
        border: '1px solid rgba(16, 185, 129, 0.3)',
      };
    case PromptStatus.Failed:
      return {
        backgroundColor: 'rgba(239, 68, 68, 0.15)',
        color: '#ef4444',
        border: '1px solid rgba(239, 68, 68, 0.3)',
      };
    default:
      return {};
  }
};

const getStatusLabel = (status: PromptStatus): string => {
  switch (status) {
    case PromptStatus.Pending:
      return 'Pending';
    case PromptStatus.Processing:
      return 'Processing';
    case PromptStatus.Completed:
      return 'Completed';
    case PromptStatus.Failed:
      return 'Failed';
    default:
      return 'Unknown';
  }
};

export default function PromptItem({ prompt, onDelete }: PromptItemProps) {
  const [expanded, setExpanded] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDelete = async () => {
    if (isDeleting) return;
    setIsDeleting(true);
    try {
      await onDelete(prompt.id);
    } catch (error) {
      console.error('Failed to delete prompt:', error);
    } finally {
      setIsDeleting(false);
    }
  };

  const hasResult = prompt.status === PromptStatus.Completed && prompt.outputText;
  const hasError = prompt.status === PromptStatus.Failed && prompt.errorMessage;
  const showExpandButton = hasResult || hasError;

  return (
    <Paper
      elevation={0}
      sx={{
        p: 3,
        mb: 2,
        backgroundColor: 'rgba(30, 41, 59, 0.6)',
        backdropFilter: 'blur(10px)',
        WebkitBackdropFilter: 'blur(10px)',
        border: '1px solid rgba(148, 163, 184, 0.1)',
        animation: 'fadeIn 0.3s ease-in',
        '@keyframes fadeIn': {
          from: {
            opacity: 0,
            transform: 'translateY(8px)',
          },
          to: {
            opacity: 1,
            transform: 'translateY(0)',
          },
        },
        '&:hover': {
          borderColor: 'rgba(148, 163, 184, 0.2)',
          transform: 'translateY(-2px)',
        },
      }}
    >
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
        <Box sx={{ flex: 1, mr: 2 }}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 1 }}>
            <Chip
              label={getStatusLabel(prompt.status)}
              size="small"
              sx={{
                ...getStatusChipStyles(prompt.status),
                fontWeight: 500,
                fontSize: '0.75rem',
                height: 24,
              }}
            />
            {showExpandButton && (
              <IconButton
                size="small"
                onClick={() => setExpanded(!expanded)}
                sx={{ color: 'text.secondary' }}
              >
                {expanded ? <ExpandLessIcon /> : <ExpandMoreIcon />}
              </IconButton>
            )}
          </Box>
          <Typography
            variant="body1"
            sx={{
              color: 'text.primary',
              whiteSpace: 'pre-wrap',
              wordBreak: 'break-word',
            }}
          >
            {prompt.text}
          </Typography>
        </Box>
        <Tooltip title="Delete prompt">
          <IconButton
            onClick={handleDelete}
            disabled={isDeleting}
            sx={{
              color: 'text.secondary',
              transition: 'all 0.2s cubic-bezier(0.4, 0, 0.2, 1)',
              '&:hover:not(:disabled)': {
                color: '#ef4444',
                backgroundColor: 'rgba(239, 68, 68, 0.1)',
                transform: 'scale(1.1)',
              },
              '&:disabled': {
                opacity: 0.5,
              },
            }}
          >
            <DeleteIcon />
          </IconButton>
        </Tooltip>
      </Box>

      <Collapse in={expanded} timeout="auto">
        <Box
          sx={{
            mt: 2,
            p: 2,
            backgroundColor: 'rgba(15, 23, 42, 0.5)',
            backdropFilter: 'blur(8px)',
            WebkitBackdropFilter: 'blur(8px)',
            borderRadius: 2,
            border: '1px solid rgba(148, 163, 184, 0.1)',
            transition: 'all 0.2s ease',
          }}
        >
          {hasResult && (
            <Box>
              <Typography 
                variant="subtitle2" 
                gutterBottom
                sx={{ 
                  color: '#10b981',
                  fontWeight: 600,
                  mb: 1,
                }}
              >
                Result:
              </Typography>
              <Typography
                variant="body2"
                sx={{
                  color: 'text.primary',
                  whiteSpace: 'pre-wrap',
                  wordBreak: 'break-word',
                  lineHeight: 1.6,
                }}
              >
                {prompt.outputText}
              </Typography>
            </Box>
          )}
          {hasError && (
            <Box>
              <Typography 
                variant="subtitle2" 
                gutterBottom
                sx={{ 
                  color: '#ef4444',
                  fontWeight: 600,
                  mb: 1,
                }}
              >
                Error:
              </Typography>
              <Typography
                variant="body2"
                sx={{
                  color: '#ef4444',
                  whiteSpace: 'pre-wrap',
                  wordBreak: 'break-word',
                  lineHeight: 1.6,
                }}
              >
                {prompt.errorMessage}
              </Typography>
            </Box>
          )}
        </Box>
      </Collapse>
    </Paper>
  );
}
