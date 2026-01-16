'use client';

import { Box, Typography, IconButton, Tooltip } from '@mui/material';
import { AutoAwesome, Refresh } from '@mui/icons-material';

interface HeaderProps {
  promptCount: number;
  onRefresh: () => void;
}

export default function Header({ promptCount, onRefresh }: HeaderProps) {
  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'flex-start',
        mb: 4,
      }}
    >
      <Box>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
          <AutoAwesome sx={{ fontSize: 28, color: 'primary.main' }} />
          <Typography variant="h4" component="h1" sx={{ fontWeight: 600 }}>
            LLM Dashboard
          </Typography>
        </Box>
        <Typography variant="body2" color="text.secondary">
          {promptCount} prompt{promptCount !== 1 ? 's' : ''} • Auto-refreshing
        </Typography>
      </Box>
      <Tooltip title="Refresh">
        <IconButton
          onClick={onRefresh}
          sx={{
            color: 'text.primary',
            '&:hover': {
              backgroundColor: 'action.hover',
            },
          }}
        >
          <Refresh />
        </IconButton>
      </Tooltip>
    </Box>
  );
}
